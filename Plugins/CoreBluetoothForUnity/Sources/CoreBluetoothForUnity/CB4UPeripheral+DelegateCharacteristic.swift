import CoreBluetooth

extension CB4UPeripheral {
    public func characteristicProperties(_ serviceUUID: CBUUID, _ characteristicUUID: CBUUID) -> Int32 {
        guard let service = peripheral.services?.first(where: { $0.uuid == serviceUUID }) else {
            return serviceNotFound
        }
        guard let characteristic = service.characteristics?.first(where: { $0.uuid == characteristicUUID }) else {
            return characteristicNotFound
        }
        
        return Int32(characteristic.properties.rawValue)
    }
}
