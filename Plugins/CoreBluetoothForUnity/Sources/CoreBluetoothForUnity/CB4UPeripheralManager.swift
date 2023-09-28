import Foundation
import CoreBluetooth

public class CB4UPeripheralManager : NSObject {
    private var peripheralManager : CBPeripheralManager!

    public var didUpdateStateHandler: CB4UCentralManagerDidUpdateStateHandler?

    public override init() {
        super.init()

        peripheralManager = CBPeripheralManager(delegate: self, queue: nil)
    }

    func selfPointer() -> UnsafeMutableRawPointer {
        return Unmanaged.passUnretained(self).toOpaque()
    }
}

extension CB4UPeripheralManager : CBPeripheralManagerDelegate {

    // MARK: - CBPeripheralManagerDelegate

    public func peripheralManagerDidUpdateState(_ peripheral: CBPeripheralManager) {
        didUpdateStateHandler?(selfPointer(), Int32(peripheral.state.rawValue))
    }
}
