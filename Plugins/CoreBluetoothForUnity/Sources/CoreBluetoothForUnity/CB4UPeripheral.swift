import CoreBluetooth

public class CB4UPeripheral : NSObject {
    var peripheral: CBPeripheral!
    
    public var didDiscoverServicesHandler: CB4UPeripheralDidDiscoverServicesHandler?
    public var didDiscoverCharacteristicsHandler: CB4UPeripheralDidDiscoverCharacteristicsHandler?
    public var didUpdateValueForCharacteristicHandler: CB4UPeripheralDidUpdateValueForCharacteristicHandler?
    public var didWriteValueForCharacteristicHandler: CB4UPeripheralDidWriteValueForCharacteristicHandler?
    public var isReadyToSendWriteWithoutResponseHandler: CB4UPeripheralIsReadyToSendWriteWithoutResponseHandler?
    public var didUpdateNotificationStateForCharacteristicHandler: CB4UPeripheralDidUpdateNotificationStateForCharacteristicHandler?
    
    let success: Int32 = 0
    let serviceNotFound: Int32 = -2
    let characteristicNotFound: Int32 = -3
    
    public init(peripheral: CBPeripheral) {
        self.peripheral = peripheral
    }
    
    func selfPointer() -> UnsafeMutableRawPointer {
        return Unmanaged.passUnretained(self).toOpaque()
    }
    
    // NOTE: code 0 is unknown error. so if error is nil, return -1.
    func errorToCode(_ error: Error?) -> Int32 {
        if error == nil {
            return -1
        }
        
        if let error = error as? CBError {
            return Int32(error.errorCode)
        }
        
        return Int32(error!._code)
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
    
    public func maximumWriteValueLength(_ writeType: CBCharacteristicWriteType) -> Int32 {
        return Int32(peripheral.maximumWriteValueLength(for: writeType))
    }
    
    public func setNotifyValue(_ serviceUUID: CBUUID, _ characteristicUUID: CBUUID, _ enabled: Bool) -> Int32 {
        return actionForCharacteristic(serviceUUID, characteristicUUID) { (service, characteristic) -> Void in
            peripheral.setNotifyValue(enabled, for: characteristic)
        }
    }
    
    public var state: CBPeripheralState {
        return peripheral.state
    }
    
    public var canSendWriteWithoutResponse: Bool {
        return peripheral.canSendWriteWithoutResponse
    }
}

extension CB4UPeripheral : CBPeripheralDelegate {
    
    // MARK: - CB4UPeripheralDelegate
    
    public func peripheral(_ peripheral: CBPeripheral, didDiscoverServices error: Error?) {
        let commaSeparatedServiceIds = peripheral.services?.map { $0.uuid.uuidString }.joined(separator: ",") ?? ""
        
        commaSeparatedServiceIds.withCString { (commaSeparatedServiceIdsCString) in
            didDiscoverServicesHandler?(selfPointer(), commaSeparatedServiceIdsCString, errorToCode(error))
        }
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didDiscoverCharacteristicsFor service: CBService, error: Error?) {
        let serviceId = service.uuid.uuidString
        let commaSeparatedCharacteristicIds = service.characteristics?.map { $0.uuid.uuidString }.joined(separator: ",") ?? ""
        
        serviceId.withCString { (serviceIdCString) in
            commaSeparatedCharacteristicIds.withCString { (commaSeparatedCharacteristicIdsCString) in
                didDiscoverCharacteristicsHandler?(selfPointer(), serviceIdCString, commaSeparatedCharacteristicIdsCString, errorToCode(error))
            }
        }
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didUpdateValueFor characteristic: CBCharacteristic, error: Error?) {
        let serviceId = characteristic.service?.uuid.uuidString ?? ""
        let characteristicId = characteristic.uuid.uuidString
        let value = characteristic.value ?? Data()
        
        serviceId.withCString { (serviceIdCString) in
            characteristicId.withCString { (characteristicIdCString) in
                value.withUnsafeBytes { (valueBytes: UnsafeRawBufferPointer) in
                    let bytes = valueBytes.bindMemory(to: UInt8.self).baseAddress!
                    didUpdateValueForCharacteristicHandler?(selfPointer(), serviceIdCString, characteristicIdCString, bytes, Int32(value.count), errorToCode(error))
                }
            }
        }
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didWriteValueFor characteristic: CBCharacteristic, error: Error?) {
        let serviceId = characteristic.service?.uuid.uuidString ?? ""
        let characteristicId = characteristic.uuid.uuidString
        
        serviceId.withCString { (serviceIdCString) in
            characteristicId.withCString { (characteristicIdCString) in
                didWriteValueForCharacteristicHandler?(selfPointer(), serviceIdCString, characteristicIdCString, errorToCode(error))
            }
        }
    }
    
    public func peripheralIsReady(toSendWriteWithoutResponse peripheral: CBPeripheral) {
        isReadyToSendWriteWithoutResponseHandler?(selfPointer())
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didUpdateNotificationStateFor characteristic: CBCharacteristic, error: Error?) {
        let serviceId = characteristic.service?.uuid.uuidString ?? ""
        let characteristicId = characteristic.uuid.uuidString
        let notificationState = characteristic.isNotifying ? 1 : 0
        
        serviceId.withCString { (serviceIdCString) in
            characteristicId.withCString { (characteristicIdCString) in
                didUpdateNotificationStateForCharacteristicHandler?(selfPointer(), serviceIdCString, characteristicIdCString, Int32(notificationState), errorToCode(error))
            }
        }
    }
}
