import CoreBluetooth

public class CB4UCentral {
    var central : CBCentral!
    
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

extension CB4UCentral: CustomStringConvertible {
    public var description: String {
        return central.description
    }
}
