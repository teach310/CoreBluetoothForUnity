import Foundation
import CoreBluetooth

// This is a wrapper class for exposing CBCentralManager to Unity.
public class CB4UCentralManager : NSObject {
    private var centralManager: CBCentralManager!
    
    public var didDiscoverPeripheralHandler: CB4UCentralManagerDidDiscoverPeripheralHandler?
    public var didUpdateStateHandler: CB4UCentralManagerDidUpdateStateHandler?
    
    public override init() {
        super.init()
        
        centralManager = CBCentralManager(delegate: self, queue: nil)
    }
    
    func selfPointer() -> UnsafeMutableRawPointer {
        return Unmanaged.passUnretained(self).toOpaque()
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
    
    public func centralManager(_ central: CBCentralManager, didDiscover peripheral: CBPeripheral, advertisementData: [String : Any], rssi RSSI: NSNumber) {        
        let peripheralId = peripheral.identifier.uuidString
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
