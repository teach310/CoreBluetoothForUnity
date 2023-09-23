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
    
    public func peripheralDiscoverCharacteristics(_ peripheralId: String, _ serviceUUID: CBUUID, _ characteristicUUIDs: [CBUUID]?) -> Int32 {
        return delegatePeripheral(peripheralId) { (peripheral) -> Int32 in
            guard let service = peripheral.services?.first(where: { $0.uuid == serviceUUID }) else {
                return serviceNotFound
            }
            
            peripheral.discoverCharacteristics(characteristicUUIDs, for: service)
            return success
        }
    }
    
    public func peripheralState(_ peripheralId: String) -> Int32 {
        return delegatePeripheral(peripheralId) { (peripheral) -> Int32 in
            return Int32(peripheral.state.rawValue)
        }
    }
}
