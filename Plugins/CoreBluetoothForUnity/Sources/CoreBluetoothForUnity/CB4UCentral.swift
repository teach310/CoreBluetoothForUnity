import CoreBluetooth

public class CB4UCentral {
    private var central : CBCentral!
    
    public init(central: CBCentral) {
        self.central = central
    }
    
    public var identifier: String {
        return central.identifier.uuidString
    }
    
    public var maximumUpdateValueLength: Int {
        return central.maximumUpdateValueLength
    }
}
