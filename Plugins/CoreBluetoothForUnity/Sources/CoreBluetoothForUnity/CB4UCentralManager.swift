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
        let peripheralId = peripheral.identifier.uuidString
        peripheral.delegate = self
        peripherals[peripheralId] = peripheral
        peripheralId.withCString { (uuidCString) in
            (peripheral.name ?? "").withCString { (nameCString) in
                didDiscoverPeripheralHandler?(selfPointer(), uuidCString, nameCString, Int32(RSSI.intValue))
            }
        }
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
}
