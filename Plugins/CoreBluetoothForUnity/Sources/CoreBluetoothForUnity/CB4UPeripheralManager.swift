import Foundation
import CoreBluetooth

public class CB4UPeripheralManager : NSObject {
    private var peripheralManager : CBPeripheralManager!
    
    public var didUpdateStateHandler: CB4UCentralManagerDidUpdateStateHandler?
    public var didAddServiceHandler: CB4UPeripheralManagerDidAddServiceHandler?
    public var didStartAdvertisingHandler: CB4UPeripheralManagerDidStartAdvertisingHandler?
    public var didSubscribeToCharacteristicHandler: CB4UPeripheralManagerDidSubscribeToCharacteristicHandler?
    public var didReceiveReadRequestHandler: CB4UPeripheralManagerDidReceiveReadRequestHandler?
    public var didReceiveWriteRequestsHandler: CB4UPeripheralManagerDidReceiveWriteRequestsHandler?
    
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
        peripheralManager.add(service.service)
    }
    
    public func startAdvertising(_ options: StartAdvertisingOptions?) {
        peripheralManager.startAdvertising(options?.advertisementData())
    }
    
    public func stopAdvertising() {
        peripheralManager.stopAdvertising()
    }
    
    public var isAdvertising: Bool {
        return peripheralManager.isAdvertising
    }

    public func respond(to request: CB4UATTRequest, withResult result: CBATTError.Code) {
        peripheralManager.respond(to: request.request, withResult: result)
    }
}

extension CB4UPeripheralManager : CBPeripheralManagerDelegate {
    
    // MARK: - CBPeripheralManagerDelegate
    
    public func peripheralManagerDidUpdateState(_ peripheral: CBPeripheralManager) {
        didUpdateStateHandler?(selfPointer(), Int32(peripheral.state.rawValue))
    }
    
    public func peripheralManager(_ peripheral: CBPeripheralManager, didAdd service: CBService, error: Error?) {
        let serviceId = service.uuid.uuidString
        
        serviceId.withCString { (serviceIdCString) in
            didAddServiceHandler?(selfPointer(), serviceIdCString, errorToCode(error))
        }
    }
    
    public func peripheralManagerDidStartAdvertising(_ peripheral: CBPeripheralManager, error: Error?) {
        didStartAdvertisingHandler?(selfPointer(), errorToCode(error))
    }

    public func peripheralManager(_ peripheral: CBPeripheralManager, central: CBCentral, didSubscribeTo characteristic: CBCharacteristic) {
        let central = CB4UCentral(central: central)
        let centralPtr = Unmanaged.passRetained(central).toOpaque()
        
        let serviceId = characteristic.service?.uuid.uuidString ?? ""
        let characteristicId = characteristic.uuid.uuidString

        serviceId.withCString { (serviceIdCString) in
            characteristicId.withCString { (characteristicIdCString) in
                didSubscribeToCharacteristicHandler?(selfPointer(), centralPtr, serviceIdCString, characteristicIdCString)
            }
        }
    }

    public func peripheralManager(_ peripheral: CBPeripheralManager, didReceiveRead request: CBATTRequest) {
        let request = CB4UATTRequest(request: request)
        let requestPtr = Unmanaged.passRetained(request).toOpaque()
        
        didReceiveReadRequestHandler?(selfPointer(), requestPtr)
    }

    public func peripheralManager(_ peripheral: CBPeripheralManager, didReceiveWrite requests: [CBATTRequest]) {
        let requests = CB4UATTRequests(requests: requests)
        let requestsPtr = Unmanaged.passRetained(requests).toOpaque()
        
        didReceiveWriteRequestsHandler?(selfPointer(), requestsPtr)
    }
}
