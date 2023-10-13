import CoreBluetooth

public class CB4UPeripheral : NSObject {
    var peripheral: CBPeripheral!
    
    let success: Int32 = 0
    let serviceNotFound: Int32 = -2
    let characteristicNotFound: Int32 = -3
    
    public init(peripheral: CBPeripheral) {
        self.peripheral = peripheral
    }
    
    public var identifier: String {
        return peripheral.identifier.uuidString
    }
    
    public var name: String? {
        return peripheral.name
    }
    
    public func discoverServices(_ serviceUUIDs: [CBUUID]?) {
        peripheral.discoverServices(serviceUUIDs)
    }
    
    public func discoverCharacteristics(_ serviceUUID: CBUUID, _ characteristicUUIDs: [CBUUID]?) -> Int32 {
        guard let service = peripheral.services?.first(where: { $0.uuid == serviceUUID }) else {
            return serviceNotFound
        }
        
        peripheral.discoverCharacteristics(characteristicUUIDs, for: service)
        return success
    }
    
    private func actionForCharacteristic(_ serviceUUID: CBUUID, _ characteristicUUID: CBUUID, _ action: (CBService, CBCharacteristic) -> Void) -> Int32 {
        guard let service = peripheral.services?.first(where: { $0.uuid == serviceUUID }) else {
            return serviceNotFound
        }
        
        guard let characteristic = service.characteristics?.first(where: { $0.uuid == characteristicUUID }) else {
            return characteristicNotFound
        }
        
        action(service, characteristic)
        return success
    }
    
    public func readCharacteristicValue(_ serviceUUID: CBUUID, _ characteristicUUID: CBUUID) -> Int32 {
        return actionForCharacteristic(serviceUUID, characteristicUUID) { (service, characteristic) -> Void in
            peripheral.readValue(for: characteristic)
        }
    }
    
    public func writeCharacteristicValue(_ serviceUUID: CBUUID, _ characteristicUUID: CBUUID, _ value: Data, _ type: CBCharacteristicWriteType) -> Int32 {
        return actionForCharacteristic(serviceUUID, characteristicUUID) { (service, characteristic) -> Void in
            peripheral.writeValue(value, for: characteristic, type: type)
        }
    }
    
    public func setNotifyValue(_ serviceUUID: CBUUID, _ characteristicUUID: CBUUID, _ enabled: Bool) -> Int32 {
        return actionForCharacteristic(serviceUUID, characteristicUUID) { (service, characteristic) -> Void in
            peripheral.setNotifyValue(enabled, for: characteristic)
        }
    }
    
    public var state: CBPeripheralState {
        return peripheral.state
    }
}
