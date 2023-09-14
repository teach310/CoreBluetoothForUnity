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
}

