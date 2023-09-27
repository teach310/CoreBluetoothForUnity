//
//  BLEPeripheral.swift
//  CB4UNativeApp
//
//  Created by Taichi Sato on 2023/09/14.
//

import Foundation
import CoreBluetooth

class BLEPeripheral: NSObject, CBPeripheralManagerDelegate {
    private var peripheralManager: CBPeripheralManager!
    private var serviceUUID: CBUUID!
    private var characteristicUUID: CBUUID!
    
    private var characteristic: CBMutableCharacteristic?
    
    override init() {
        super.init()
        peripheralManager = CBPeripheralManager(delegate: self, queue: nil)
        serviceUUID = CBUUID(string: "068C47B7-FC04-4D47-975A-7952BE1A576F")
        characteristicUUID = CBUUID(string: "E3737B3F-A08D-405B-B32D-35A8F6C64C5D")
    }
    
    func cbManagerStateName(_ state: CBManagerState) -> String {
        switch state {
        case .unknown:
            return "unknown"
        case .resetting:
            return "resetting"
        case .unsupported:
            return "unsupported"
        case .unauthorized:
            return "unauthorized"
        case .poweredOff:
            return "poweredOff"
        case .poweredOn:
            return "poweredOn"
        @unknown default:
            return "unknown default"
        }
    }
    
    private func sampleData() -> Data {
        let value = "data is " + String(format: "%03d", Int.random(in: 100..<1000))
        return value.data(using: .utf8)!
    }
    
    // MARK: - CBPeripheralManagerDelegate
    
    public func peripheralManagerDidUpdateState(_ peripheral: CBPeripheralManager) {
        print("peripheralManagerDidUpdateState: \(cbManagerStateName(peripheral.state))")
        
        if peripheral.state == .poweredOn {
            let service = CBMutableService(type: serviceUUID, primary: true)
            characteristic = CBMutableCharacteristic(
                type: characteristicUUID,
                properties: [.read, .write, .notify],
                value: nil,
                permissions: [.readable, .writeable]
            )
            
            service.characteristics = [characteristic!]
            peripheral.add(service)
        }
    }
    
    public func peripheralManager(_ peripheral: CBPeripheralManager, didAdd service: CBService, error: Error?) {
        print("didAdd service: \(service.uuid.uuidString)")
        
        if let error = error {
            print("error: \(error.localizedDescription)")
            return
        }
        
        let advertisementData = [CBAdvertisementDataServiceUUIDsKey: [serviceUUID]]
        peripheral.startAdvertising(advertisementData)
    }
    
    public func peripheralManagerDidStartAdvertising(_ peripheral: CBPeripheralManager, error: Error?) {
        print("peripheralManagerDidStartAdvertising")
        
        if let error = error {
            print("error: \(error.localizedDescription)")
            return
        }
    }
    
    public func peripheralManager(_ peripheral: CBPeripheralManager, didReceiveRead request: CBATTRequest) {
        print("didReceiveRead")
        
        if !request.characteristic.uuid.isEqual(characteristicUUID) {
            peripheral.respond(to: request, withResult: .attributeNotFound)
            return
        }
        
        if request.offset > characteristicUUID.data.count {
            peripheral.respond(to: request, withResult: .invalidOffset)
            return
        }
        
        if characteristic?.value == nil {
            characteristic?.value = sampleData()
        }
        
        request.value = characteristic?.value
        peripheral.respond(to: request, withResult: .success)
    }
    
    public func peripheralManager(_ peripheral: CBPeripheralManager, didReceiveWrite requests: [CBATTRequest]) {
        print("didReceiveWrite")
        
        for request in requests {
            if !request.characteristic.uuid.isEqual(characteristicUUID) {
                peripheral.respond(to: request, withResult: .attributeNotFound)
                return
            }
            
            if request.offset > characteristicUUID.data.count {
                peripheral.respond(to: request, withResult: .invalidOffset)
                return
            }
            
            if let data = request.value {
                print("data: \(String(data: data, encoding: .utf8) ?? "")")
                
                characteristic?.value = data
                peripheral.updateValue(data, for: characteristic!, onSubscribedCentrals: nil)
            }
            
            peripheral.respond(to: request, withResult: .success)
        }
    }
}
