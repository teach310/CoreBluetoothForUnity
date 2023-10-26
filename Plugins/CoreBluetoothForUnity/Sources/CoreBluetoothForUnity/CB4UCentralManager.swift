import Foundation
import CoreBluetooth

// This is a wrapper class for exposing CBCentralManager to Unity.
public class CB4UCentralManager : NSObject {
    private var centralManager: CBCentralManager!
    
    public var didConnectHandler: CB4UCentralManagerDidConnectHandler?
    public var didDisconnectPeripheralHandler: CB4UCentralManagerDidDisconnectPeripheralHandler?
    public var didFailToConnectHandler: CB4UCentralManagerDidFailToConnectHandler?
    public var didDiscoverPeripheralHandler: CB4UCentralManagerDidDiscoverPeripheralHandler?
    public var didUpdateStateHandler: CB4UCentralManagerDidUpdateStateHandler?
    
    public init(_ options: [String: Any]? = nil) {
        super.init()
        
        centralManager = CBCentralManager(delegate: self, queue: nil, options: options)
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
    
    public func connect(peripheral: CB4UPeripheral) {
        centralManager.connect(peripheral.peripheral)
    }
    
    public func cancelPeripheralConnection(peripheral: CB4UPeripheral) {
        centralManager.cancelPeripheralConnection(peripheral.peripheral)
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
        peripheral.delegate = cb4u_peripheral
        let peripheralPtr = Unmanaged.passRetained(cb4u_peripheral).toOpaque()
        didDiscoverPeripheralHandler?(selfPointer(), peripheralPtr, Int32(RSSI.intValue))
    }
    
    public func centralManagerDidUpdateState(_ central: CBCentralManager) {
        didUpdateStateHandler?(selfPointer(), Int32(central.state.rawValue))
    }
}
