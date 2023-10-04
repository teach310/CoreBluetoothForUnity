import CoreBluetooth

public class StartAdvertisingOptions {
    public var localName: String?
    public var serviceUUIDs: [CBUUID]?
    
    public init() {
    }
    
    public init(localName: String?, serviceUUIDs: [CBUUID]?) {
        self.localName = localName
        self.serviceUUIDs = serviceUUIDs
    }
    
    public func advertisementData() -> [String: Any]? {
        var dict: [String: Any] = [:]
        if let localName = localName {
            dict[CBAdvertisementDataLocalNameKey] = localName
        }
        if let serviceUUIDs = serviceUUIDs {
            dict[CBAdvertisementDataServiceUUIDsKey] = serviceUUIDs
        }
        
        return dict.isEmpty ? nil : dict
    }
}
