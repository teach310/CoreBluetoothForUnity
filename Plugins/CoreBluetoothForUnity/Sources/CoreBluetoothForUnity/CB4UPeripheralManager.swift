import Foundation
import CoreBluetooth

public class CB4UPeripheralManager : NSObject {
    private var peripheralManager : CBPeripheralManager!

    public var didUpdateStateHandler: CB4UCentralManagerDidUpdateStateHandler?
    public var didAddServiceHandler: CB4UPeripheralManagerDidAddServiceHandler?

    public override init() {
        super.init()

        peripheralManager = CBPeripheralManager(delegate: self, queue: nil)
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

    public func add(_ service: CB4UMutableService) {
        print("addService: \(service.service.uuid.uuidString)")
        peripheralManager.add(service.service)
    }

    public func startAdvertising(_ options: StartAdvertisingOptions?) {
        peripheralManager.startAdvertising(options?.advertisementData())
    }
}

extension CB4UPeripheralManager : CBPeripheralManagerDelegate {

    // MARK: - CBPeripheralManagerDelegate

    public func peripheralManagerDidUpdateState(_ peripheral: CBPeripheralManager) {
        didUpdateStateHandler?(selfPointer(), Int32(peripheral.state.rawValue))
    }

    public func peripheralManager(_ peripheral: CBPeripheralManager, didAdd service: CBService, error: Error?) {
        let serviceId = service.uuid.uuidString
        print("didAddService: \(serviceId)")

        serviceId.withCString { (serviceIdCString) in
            didAddServiceHandler?(selfPointer(), serviceIdCString, errorToCode(error))
        }
    }
}
