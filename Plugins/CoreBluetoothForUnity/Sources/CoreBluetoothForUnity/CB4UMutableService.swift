import CoreBluetooth

public class CB4UMutableService {
    var service: CBMutableService!
    
    public init(service: CBMutableService) {
        self.service = service
    }
    
    public func setCharacteristics(_ characteristics: [CB4UMutableCharacteristic]?) {
        if let characteristics = characteristics {
            service.characteristics = characteristics.map { $0.characteristic }
        } else {
            service.characteristics = nil
        }
    }
}
