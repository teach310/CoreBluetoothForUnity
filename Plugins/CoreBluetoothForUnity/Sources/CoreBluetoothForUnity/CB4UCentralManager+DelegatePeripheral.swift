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
    
    private func delegatePeripheralForCharacteristic(_ peripheralId: String, _ serviceUUID: CBUUID, _ characteristicUUID: CBUUID, _ action: (CBPeripheral, CBService, CBCharacteristic) -> Void) -> Int32 {
        return delegatePeripheral(peripheralId) { (peripheral) -> Int32 in
            guard let service = peripheral.services?.first(where: { $0.uuid == serviceUUID }) else {
                return serviceNotFound
            }
            
            guard let characteristic = service.characteristics?.first(where: { $0.uuid == characteristicUUID }) else {
                return characteristicNotFound
            }
            
            action(peripheral, service, characteristic)
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
    
    public func peripheralReadCharacteristicValue(_ peripheralId: String, _ serviceUUID: CBUUID, _ characteristicUUID: CBUUID) -> Int32 {
        return delegatePeripheralForCharacteristic(peripheralId, serviceUUID, characteristicUUID) { (peripheral, service, characteristic) -> Void in
            peripheral.readValue(for: characteristic)
        }
    }
    
    public func peripheralWriteCharacteristicValue(_ peripheralId: String, _ serviceUUID: CBUUID, _ characteristicUUID: CBUUID, _ value: Data, _ type: CBCharacteristicWriteType) -> Int32 {
        return delegatePeripheralForCharacteristic(peripheralId, serviceUUID, characteristicUUID) { (peripheral, service, characteristic) -> Void in
            peripheral.writeValue(value, for: characteristic, type: type)
        }
    }
    
    public func peripheralSetNotifyValue(_ peripheralId: String, _ serviceUUID: CBUUID, _ characteristicUUID: CBUUID, _ enabled: Bool) -> Int32 {
        return delegatePeripheralForCharacteristic(peripheralId, serviceUUID, characteristicUUID) { (peripheral, service, characteristic) -> Void in
            peripheral.setNotifyValue(enabled, for: characteristic)
        }
    }
    
    public func peripheralState(_ peripheralId: String) -> Int32 {
        return delegatePeripheral(peripheralId) { (peripheral) -> Int32 in
            return Int32(peripheral.state.rawValue)
        }
    }
}
