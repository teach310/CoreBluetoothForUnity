import CoreBluetooth

extension CB4UCentralManager {
    public func characteristicProperties(_ peripheralId: String, _ serviceUUID: CBUUID, _ characteristicUUID: CBUUID) -> Int32 {
        guard let peripheral = peripherals[peripheralId] else {
            return peripheralNotFound
        }
        guard let service = peripheral.services?.first(where: { $0.uuid == serviceUUID }) else {
            return serviceNotFound
        }
        guard let characteristic = service.characteristics?.first(where: { $0.uuid == characteristicUUID }) else {
            return characteristicNotFound
        }
        
        return Int32(characteristic.properties.rawValue)
    }
}
