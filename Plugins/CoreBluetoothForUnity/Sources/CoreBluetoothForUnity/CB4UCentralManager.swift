import Foundation
import CoreBluetooth

// This is a wrapper class for exposing CBCentralManager to Unity.
public class CB4UCentralManager : NSObject {
    private var centralManager: CBCentralManager!
    // key: peripheralId
    var peripherals: Dictionary<String, CBPeripheral> = [:]
    
    public var didConnectHandler: CB4UCentralManagerDidConnectHandler?
    public var didDisconnectPeripheralHandler: CB4UCentralManagerDidDisconnectPeripheralHandler?
    public var didFailToConnectHandler: CB4UCentralManagerDidFailToConnectHandler?
    public var didDiscoverPeripheralHandler: CB4UCentralManagerDidDiscoverPeripheralHandler?
    public var didUpdateStateHandler: CB4UCentralManagerDidUpdateStateHandler?
    
    public var peripheralDidDiscoverServicesHandler: CB4UPeripheralDidDiscoverServicesHandler?
    public var peripheralDidDiscoverCharacteristicsHandler: CB4UPeripheralDidDiscoverCharacteristicsHandler?
    public var peripheralDidUpdateValueForCharacteristicHandler: CB4UPeripheralDidUpdateValueForCharacteristicHandler?
    public var peripheralDidWriteValueForCharacteristicHandler: CB4UPeripheralDidWriteValueForCharacteristicHandler?
    public var peripheralDidUpdateNotificationStateForCharacteristicHandler: CB4UPeripheralDidUpdateNotificationStateForCharacteristicHandler?
    
    let peripheralNotFound: Int32 = -1
    let serviceNotFound: Int32 = -2
    let characteristicNotFound: Int32 = -3
    let success: Int32 = 0
    
    public override init() {
        super.init()
        
        centralManager = CBCentralManager(delegate: self, queue: nil)
    }
    
    func selfPointer() -> UnsafeMutableRawPointer {
        return Unmanaged.passUnretained(self).toOpaque()
    }
    
    // NOTE: code 0 is unknown error. so if error is nil, return -1.
    func errorToCode(_ error: Error?) -> Int32 {
        if error == nil {
            return -1
        }
        
        if let error = error as? CBError {
            return Int32(error.errorCode)
        }
        
        return Int32(error!._code)
    }
    
    public func connect(peripheralId: String) -> Int32 {
        guard let peripheral = peripherals[peripheralId] else {
            return peripheralNotFound
        }
        centralManager.connect(peripheral)
        return success
    }
    
    public func cancelPeripheralConnection(peripheralId: String) -> Int32 {
        guard let peripheral = peripherals[peripheralId] else {
            return peripheralNotFound
        }
        centralManager.cancelPeripheralConnection(peripheral)
        return success
    }
    
    public func scanForPeripherals(withServices serviceUUIDs: [CBUUID]?) {
        centralManager.scanForPeripherals(withServices: serviceUUIDs)
    }
    
    public func stopScan() {
        centralManager.stopScan()
    }
    
    public var isScanning: Bool {
        return centralManager.isScanning
    }
}

extension CB4UCentralManager : CBCentralManagerDelegate {
    
    // MARK: - CBCentralManagerDelegate
    
    public func centralManager(_ central: CBCentralManager, didConnect peripheral: CBPeripheral) {
        let peripheralId = peripheral.identifier.uuidString
        peripheralId.withCString { (uuidCString) in
            didConnectHandler?(selfPointer(), uuidCString)
        }
    }
    
    public func centralManager(_ central: CBCentralManager, didDisconnectPeripheral peripheral: CBPeripheral, error: Error?) {
        let peripheralId = peripheral.identifier.uuidString
        peripheralId.withCString { (uuidCString) in
            didDisconnectPeripheralHandler?(selfPointer(), uuidCString, errorToCode(error))
        }
    }
    
    public func centralManager(_ central: CBCentralManager, didFailToConnect peripheral: CBPeripheral, error: Error?) {
        let peripheralId = peripheral.identifier.uuidString
        peripheralId.withCString { (uuidCString) in
            didFailToConnectHandler?(selfPointer(), uuidCString, errorToCode(error))
        }
    }
    
    public func centralManager(_ central: CBCentralManager, didDiscover peripheral: CBPeripheral, advertisementData: [String : Any], rssi RSSI: NSNumber) {
        let cb4u_peripheral = CB4UPeripheral(peripheral: peripheral)
        peripheral.delegate = self
        let peripheralPtr = Unmanaged.passRetained(cb4u_peripheral).toOpaque()
        didDiscoverPeripheralHandler?(selfPointer(), peripheralPtr, Int32(RSSI.intValue))
    }
    
    public func centralManagerDidUpdateState(_ central: CBCentralManager) {
        didUpdateStateHandler?(selfPointer(), Int32(central.state.rawValue))
    }
}

extension CB4UCentralManager : CBPeripheralDelegate {
    
    // MARK: - CB4UPeripheralDelegate
    
    public func peripheral(_ peripheral: CBPeripheral, didDiscoverServices error: Error?) {
        let peripheralId = peripheral.identifier.uuidString
        let commaSeparatedServiceIds = peripheral.services?.map { $0.uuid.uuidString }.joined(separator: ",") ?? ""
        
        peripheralId.withCString { (uuidCString) in
            commaSeparatedServiceIds.withCString { (commaSeparatedServiceIdsCString) in
                peripheralDidDiscoverServicesHandler?(selfPointer(), uuidCString, commaSeparatedServiceIdsCString, errorToCode(error))
            }
        }
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didDiscoverCharacteristicsFor service: CBService, error: Error?) {
        let peripheralId = peripheral.identifier.uuidString
        let serviceId = service.uuid.uuidString
        let commaSeparatedCharacteristicIds = service.characteristics?.map { $0.uuid.uuidString }.joined(separator: ",") ?? ""
        
        peripheralId.withCString { (peripheralIdCString) in
            serviceId.withCString { (serviceIdCString) in
                commaSeparatedCharacteristicIds.withCString { (commaSeparatedCharacteristicIdsCString) in
                    peripheralDidDiscoverCharacteristicsHandler?(selfPointer(), peripheralIdCString, serviceIdCString, commaSeparatedCharacteristicIdsCString, errorToCode(error))
                }
            }
        }
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didUpdateValueFor characteristic: CBCharacteristic, error: Error?) {
        let peripheralId = peripheral.identifier.uuidString
        let serviceId = characteristic.service?.uuid.uuidString ?? ""
        let characteristicId = characteristic.uuid.uuidString
        let value = characteristic.value ?? Data()
        
        peripheralId.withCString { (peripheralIdCString) in
            serviceId.withCString { (serviceIdCString) in
                characteristicId.withCString { (characteristicIdCString) in
                    value.withUnsafeBytes { (valueBytes: UnsafeRawBufferPointer) in
                        let bytes = valueBytes.bindMemory(to: UInt8.self).baseAddress!
                        peripheralDidUpdateValueForCharacteristicHandler?(selfPointer(), peripheralIdCString, serviceIdCString, characteristicIdCString, bytes, Int32(value.count), errorToCode(error))
                    }
                }
            }
        }
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didWriteValueFor characteristic: CBCharacteristic, error: Error?) {
        let peripheralId = peripheral.identifier.uuidString
        let serviceId = characteristic.service?.uuid.uuidString ?? ""
        let characteristicId = characteristic.uuid.uuidString
        
        peripheralId.withCString { (peripheralIdCString) in
            serviceId.withCString { (serviceIdCString) in
                characteristicId.withCString { (characteristicIdCString) in
                    peripheralDidWriteValueForCharacteristicHandler?(selfPointer(), peripheralIdCString, serviceIdCString, characteristicIdCString, errorToCode(error))
                }
            }
        }
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didUpdateNotificationStateFor characteristic: CBCharacteristic, error: Error?) {
        let peripheralId = peripheral.identifier.uuidString
        let serviceId = characteristic.service?.uuid.uuidString ?? ""
        let characteristicId = characteristic.uuid.uuidString
        let notificationState = characteristic.isNotifying ? 1 : 0
        
        peripheralId.withCString { (peripheralIdCString) in
            serviceId.withCString { (serviceIdCString) in
                characteristicId.withCString { (characteristicIdCString) in
                    peripheralDidUpdateNotificationStateForCharacteristicHandler?(selfPointer(), peripheralIdCString, serviceIdCString, characteristicIdCString, Int32(notificationState), errorToCode(error))
                }
            }
        }
    }
}
