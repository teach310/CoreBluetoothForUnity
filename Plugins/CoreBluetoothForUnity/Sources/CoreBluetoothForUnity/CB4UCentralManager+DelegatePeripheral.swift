import CoreBluetooth

extension CB4UCentralManager {
    private func delegatePeripheral(_ peripheralId: String, _ action: (CBPeripheral) -> Int32) -> Int32 {
        guard let peripheral = peripherals[peripheralId] else {
            return peripheralNotFound
        }
        return action(peripheral)
    }
    
    private func delegatePeripheral(_ peripheralId: String, _ action: (CBPeripheral) -> Void) -> Int32 {
        return delegatePeripheral(peripheralId) { (peripheral) -> Int32 in
            action(peripheral)
            return success
        }
    }
    
    public func peripheralDiscoverServices(_ peripheralId: String, _ serviceUUIDs: [CBUUID]?) -> Int32 {
        return delegatePeripheral(peripheralId) { (peripheral) -> Void in
            peripheral.discoverServices(serviceUUIDs)
        }
    }
}
