import CoreBluetooth

public class CB4UMutableCharacteristic {
    var characteristic: CBMutableCharacteristic!
    
    public init(characteristic: CBMutableCharacteristic) {
        self.characteristic = characteristic
    }
}
