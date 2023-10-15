import CoreBluetooth

@_cdecl("cb4u_central_release")
public func cb4u_central_release(_ centralPtr: UnsafeRawPointer) {
    Unmanaged<CB4UCentral>.fromOpaque(centralPtr).release()
}

@_cdecl("cb4u_central_identifier")
public func cb4u_central_identifier(_ centralPtr: UnsafeRawPointer, _ identifier: UnsafeMutablePointer<CChar>, _ identifierSize: Int32) {
    let instance = Unmanaged<CB4UCentral>.fromOpaque(centralPtr).takeUnretainedValue()
    
    let identifierString = instance.identifier
    let maxSize = Int(identifierSize) - 1
    identifierString.withCString { identifierStringPtr in
        if identifierString.count > maxSize {
            print("cb4u_central_identifier: identifierSize is too small. identifierSize: \(identifierSize), identifier.count: \(identifierString.count)")
        } else {
            strcpy(identifier, identifierStringPtr)
        }
    }
}

@_cdecl("cb4u_central_maximum_update_value_length")
public func cb4u_central_maximum_update_value_length(_ centralPtr: UnsafeRawPointer) -> Int32 {
    let instance = Unmanaged<CB4UCentral>.fromOpaque(centralPtr).takeUnretainedValue()
    
    return Int32(instance.maximumUpdateValueLength)
}

@_cdecl("cb4u_central_manager_new")
public func cb4u_central_manager_new() -> UnsafeMutableRawPointer {
    return Unmanaged.passRetained(CB4UCentralManager()).toOpaque()
}

@_cdecl("cb4u_central_manager_release")
public func cb4u_central_manager_release(_ centralManagerPtr: UnsafeRawPointer) {
    Unmanaged<CB4UCentralManager>.fromOpaque(centralManagerPtr).release()
}

public typealias CB4UCentralManagerDidConnectHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>) -> Void
public typealias CB4UCentralManagerDidDisconnectPeripheralHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UCentralManagerDidFailToConnectHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UCentralManagerDidDiscoverPeripheralHandler = @convention(c) (UnsafeRawPointer, UnsafeRawPointer, Int32) -> Void
public typealias CB4UCentralManagerDidUpdateStateHandler = @convention(c) (UnsafeRawPointer, Int32) -> Void

@_cdecl("cb4u_central_manager_register_handlers")
public func cb4u_central_manager_register_handlers(
    _ centralPtr: UnsafeRawPointer,
    _ didConnectHandler: @escaping CB4UCentralManagerDidConnectHandler,
    _ didDisconnectPeripheralHandler: @escaping CB4UCentralManagerDidDisconnectPeripheralHandler,
    _ didFailToConnectHandler: @escaping CB4UCentralManagerDidFailToConnectHandler,
    _ didDiscoverPeripheralHandler: @escaping CB4UCentralManagerDidDiscoverPeripheralHandler,
    _ didUpdateStateHandler: @escaping CB4UCentralManagerDidUpdateStateHandler
) {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    instance.didConnectHandler = didConnectHandler
    instance.didDisconnectPeripheralHandler = didDisconnectPeripheralHandler
    instance.didFailToConnectHandler = didFailToConnectHandler
    instance.didDiscoverPeripheralHandler = didDiscoverPeripheralHandler
    instance.didUpdateStateHandler = didUpdateStateHandler
}

@_cdecl("cb4u_central_manager_connect")
public func cb4u_central_manager_connect(_ centralPtr: UnsafeRawPointer, _ peripheralPtr: UnsafeRawPointer) {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    let peripheral = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    instance.connect(peripheral: peripheral)
}

@_cdecl("cb4u_central_manager_cancel_peripheral_connection")
public func cb4u_central_manager_cancel_peripheral_connection(_ centralPtr: UnsafeRawPointer, _ peripheralPtr: UnsafeRawPointer) {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    let peripheral = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    instance.cancelPeripheralConnection(peripheral: peripheral)
}

@_cdecl("cb4u_central_manager_scan_for_peripherals")
public func cb4u_central_manager_scan_for_peripherals(
    _ centralPtr: UnsafeRawPointer,
    _ serviceUUIDs: UnsafePointer<UnsafePointer<CChar>?>,
    _ serviceUUIDsCount: Int32
) {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    let serviceUUIDsArray = (0..<Int(serviceUUIDsCount)).map { index -> CBUUID in
        let uuidString = String(cString: serviceUUIDs[index]!)
        return CBUUID(string: uuidString)
    }
    
    instance.scanForPeripherals(withServices: serviceUUIDsArray)
}

@_cdecl("cb4u_central_manager_stop_scan")
public func cb4u_central_manager_stop_scan(_ centralPtr: UnsafeRawPointer) {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    instance.stopScan()
}

@_cdecl("cb4u_central_manager_is_scanning")
public func cb4u_central_manager_is_scanning(_ centralPtr: UnsafeRawPointer) -> Bool {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    return instance.isScanning
}

@_cdecl("cb4u_peripheral_release")
public func cb4u_peripheral_release(_ peripheralPtr: UnsafeRawPointer) {
    Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).release()
}

public typealias CB4UPeripheralDidDiscoverServicesHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UPeripheralDidDiscoverCharacteristicsHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UPeripheralDidUpdateValueForCharacteristicHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, UnsafePointer<CChar>, UnsafePointer<UInt8>, Int32, Int32) -> Void
public typealias CB4UPeripheralDidWriteValueForCharacteristicHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UPeripheralDidUpdateNotificationStateForCharacteristicHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, UnsafePointer<CChar>, Int32, Int32) -> Void

@_cdecl("cb4u_peripheral_register_handlers")
public func cb4u_peripheral_register_handlers(
    _ peripheralPtr: UnsafeRawPointer,
    _ didDiscoverServicesHandler: @escaping CB4UPeripheralDidDiscoverServicesHandler,
    _ didDiscoverCharacteristicsHandler: @escaping CB4UPeripheralDidDiscoverCharacteristicsHandler,
    _ didUpdateValueForCharacteristicHandler: @escaping CB4UPeripheralDidUpdateValueForCharacteristicHandler,
    _ didWriteValueForCharacteristicHandler: @escaping CB4UPeripheralDidWriteValueForCharacteristicHandler,
    _ didUpdateNotificationStateForCharacteristicHandler: @escaping CB4UPeripheralDidUpdateNotificationStateForCharacteristicHandler
) {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    instance.didDiscoverServicesHandler = didDiscoverServicesHandler
    instance.didDiscoverCharacteristicsHandler = didDiscoverCharacteristicsHandler
    instance.didUpdateValueForCharacteristicHandler = didUpdateValueForCharacteristicHandler
    instance.didWriteValueForCharacteristicHandler = didWriteValueForCharacteristicHandler
    instance.didUpdateNotificationStateForCharacteristicHandler = didUpdateNotificationStateForCharacteristicHandler
}

@_cdecl("cb4u_peripheral_identifier")
public func cb4u_peripheral_identifier(_ peripheralPtr: UnsafeRawPointer, _ identifier: UnsafeMutablePointer<CChar>, _ identifierSize: Int32) {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    let identifierString = instance.identifier
    let maxSize = Int(identifierSize) - 1
    identifierString.withCString { identifierStringPtr in
        if identifierString.count > maxSize {
            print("cb4u_peripheral_identifier: identifierSize is too small. identifierSize: \(identifierSize), identifier.count: \(identifierString.count)")
        } else {
            strcpy(identifier, identifierStringPtr)
        }
    }
}

@_cdecl("cb4u_peripheral_name")
public func cb4u_peripheral_name(_ peripheralPtr: UnsafeRawPointer, _ name: UnsafeMutablePointer<CChar>, _ nameSize: Int32) -> Int32 {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    guard let nameString = instance.name else {
        return 0
    }
    
    let maxSize = Int(nameSize) - 1
    nameString.withCString { nameStringPtr in
        if nameString.count > maxSize {
            print("cb4u_peripheral_name: nameSize is too small. nameSize: \(nameSize), name.count: \(nameString.count)")
        } else {
            strcpy(name, nameStringPtr)
        }
    }
    return 1
}

@_cdecl("cb4u_peripheral_discover_services")
public func cb4u_peripheral_discover_services(
    _ peripheralPtr: UnsafeRawPointer,
    _ serviceUUIDs: UnsafePointer<UnsafePointer<CChar>?>,
    _ serviceUUIDsCount: Int32
) {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    let serviceUUIDsArray = (0..<Int(serviceUUIDsCount)).map { index -> CBUUID in
        let uuidString = String(cString: serviceUUIDs[index]!)
        return CBUUID(string: uuidString)
    }
    
    instance.discoverServices(serviceUUIDsArray)
}

@_cdecl("cb4u_peripheral_discover_characteristics")
public func cb4u_peripheral_discover_characteristics(
    _ peripheralPtr: UnsafeRawPointer,
    _ serviceUUID: UnsafePointer<CChar>,
    _ characteristicUUIDs: UnsafePointer<UnsafePointer<CChar>?>,
    _ characteristicUUIDsCount: Int32
) -> Int32 {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    let characteristicUUIDsArray = (0..<Int(characteristicUUIDsCount)).map { index -> CBUUID in
        let uuidString = String(cString: characteristicUUIDs[index]!)
        return CBUUID(string: uuidString)
    }
    
    return instance.discoverCharacteristics(CBUUID(string: String(cString: serviceUUID)), characteristicUUIDsArray)
}

@_cdecl("cb4u_peripheral_read_characteristic_value")
public func cb4u_peripheral_read_characteristic_value(
    _ peripheralPtr: UnsafeRawPointer,
    _ serviceUUID: UnsafePointer<CChar>,
    _ characteristicUUID: UnsafePointer<CChar>
) -> Int32 {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    return instance.readCharacteristicValue(CBUUID(string: String(cString: serviceUUID)), CBUUID(string: String(cString: characteristicUUID)))
}

@_cdecl("cb4u_peripheral_write_characteristic_value")
public func cb4u_peripheral_write_characteristic_value(
    _ peripheralPtr: UnsafeRawPointer,
    _ serviceUUID: UnsafePointer<CChar>,
    _ characteristicUUID: UnsafePointer<CChar>,
    _ dataBytes: UnsafePointer<UInt8>,
    _ dataLength: Int32,
    _ writeType: Int32
) -> Int32 {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    let data = Data(bytes: dataBytes, count: Int(dataLength))
    return instance.writeCharacteristicValue(CBUUID(string: String(cString: serviceUUID)), CBUUID(string: String(cString: characteristicUUID)), data, CBCharacteristicWriteType(rawValue: Int(writeType))!)
}

@_cdecl("cb4u_peripheral_set_notify_value")
public func cb4u_peripheral_set_notify_value(
    _ peripheralPtr: UnsafeRawPointer,
    _ serviceUUID: UnsafePointer<CChar>,
    _ characteristicUUID: UnsafePointer<CChar>,
    _ enabled: Bool
) -> Int32 {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    return instance.setNotifyValue(CBUUID(string: String(cString: serviceUUID)), CBUUID(string: String(cString: characteristicUUID)), enabled)
}

@_cdecl("cb4u_peripheral_state")
public func cb4u_peripheral_state(_ peripheralPtr: UnsafeRawPointer) -> Int32 {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    return Int32(instance.state.rawValue)
}

@_cdecl("cb4u_peripheral_characteristic_properties")
public func cb4u_peripheral_characteristic_properties(
    _ peripheralPtr: UnsafeRawPointer,
    _ serviceUUID: UnsafePointer<CChar>,
    _ characteristicUUID: UnsafePointer<CChar>
) -> Int32 {
    let instance = Unmanaged<CB4UPeripheral>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    return instance.characteristicProperties(CBUUID(string: String(cString: serviceUUID)), CBUUID(string: String(cString: characteristicUUID)))
}

@_cdecl("cb4u_peripheral_manager_new")
public func cb4u_peripheral_manager_new() -> UnsafeMutableRawPointer {
    return Unmanaged.passRetained(CB4UPeripheralManager()).toOpaque()
}

@_cdecl("cb4u_peripheral_manager_release")
public func cb4u_peripheral_manager_release(_ peripheralManagerPtr: UnsafeRawPointer) {
    Unmanaged<CB4UPeripheralManager>.fromOpaque(peripheralManagerPtr).release()
}

public typealias CB4UPeripheralManagerDidUpdateStateHandler = @convention(c) (UnsafeRawPointer, Int32) -> Void
public typealias CB4UPeripheralManagerDidAddServiceHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UPeripheralManagerDidStartAdvertisingHandler = @convention(c) (UnsafeRawPointer, Int32) -> Void
public typealias CB4UPeripheralManagerDidReceiveReadRequestHandler = @convention(c) (UnsafeRawPointer, UnsafeRawPointer) -> Void
public typealias CB4UPeripheralManagerDidReceiveWriteRequestsHandler = @convention(c) (UnsafeRawPointer, UnsafeRawPointer) -> Void

@_cdecl("cb4u_peripheral_manager_register_handlers")
public func cb4u_peripheral_manager_register_handlers(
    _ peripheralManagerPtr: UnsafeRawPointer,
    _ didUpdateStateHandler: @escaping CB4UPeripheralManagerDidUpdateStateHandler,
    _ didAddServiceHandler: @escaping CB4UPeripheralManagerDidAddServiceHandler,
    _ didStartAdvertisingHandler: @escaping CB4UPeripheralManagerDidStartAdvertisingHandler,
    _ didReceiveReadRequestHandler: @escaping CB4UPeripheralManagerDidReceiveReadRequestHandler,
    _ didReceiveWriteRequestsHandler: @escaping CB4UPeripheralManagerDidReceiveWriteRequestsHandler
) {
    let instance = Unmanaged<CB4UPeripheralManager>.fromOpaque(peripheralManagerPtr).takeUnretainedValue()
    
    instance.didUpdateStateHandler = didUpdateStateHandler
    instance.didAddServiceHandler = didAddServiceHandler
    instance.didStartAdvertisingHandler = didStartAdvertisingHandler
    instance.didReceiveReadRequestHandler = didReceiveReadRequestHandler
    instance.didReceiveWriteRequestsHandler = didReceiveWriteRequestsHandler
}

@_cdecl("cb4u_peripheral_manager_add_service")
public func cb4u_peripheral_manager_add_service(_ peripheralPtr: UnsafeRawPointer, _ servicePtr: UnsafeRawPointer) {
    let instance = Unmanaged<CB4UPeripheralManager>.fromOpaque(peripheralPtr).takeUnretainedValue()
    let service = Unmanaged<CB4UMutableService>.fromOpaque(servicePtr).takeUnretainedValue()
    
    instance.add(service)
}

@_cdecl("cb4u_peripheral_manager_start_advertising")
public func cb4u_peripheral_manager_start_advertising(_ peripheralPtr: UnsafeRawPointer, _ localName: UnsafePointer<CChar>?, _ serviceUUIDs: UnsafePointer<UnsafePointer<CChar>?>, _ serviceUUIDsCount: Int32) {
    let serviceUUIDsArray = (0..<Int(serviceUUIDsCount)).map { index -> CBUUID in
        let uuidString = String(cString: serviceUUIDs[index]!)
        return CBUUID(string: uuidString)
    }
    let options = StartAdvertisingOptions(localName: localName != nil ? String(cString: localName!) : nil, serviceUUIDs: serviceUUIDsArray)
    let instance = Unmanaged<CB4UPeripheralManager>.fromOpaque(peripheralPtr).takeUnretainedValue()
    instance.startAdvertising(options)
}

@_cdecl("cb4u_peripheral_manager_stop_advertising")
public func cb4u_peripheral_manager_stop_advertising(_ peripheralPtr: UnsafeRawPointer) {
    let instance = Unmanaged<CB4UPeripheralManager>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    instance.stopAdvertising()
}

@_cdecl("cb4u_peripheral_manager_is_advertising")
public func cb4u_peripheral_manager_is_advertising(_ peripheralPtr: UnsafeRawPointer) -> Bool {
    let instance = Unmanaged<CB4UPeripheralManager>.fromOpaque(peripheralPtr).takeUnretainedValue()
    
    return instance.isAdvertising
}

@_cdecl("cb4u_peripheral_manager_respond_to_request")
public func cb4u_peripheral_manager_respond_to_request(_ peripheralPtr: UnsafeRawPointer, _ requestPtr: UnsafeRawPointer, _ result: Int32) {
    let instance = Unmanaged<CB4UPeripheralManager>.fromOpaque(peripheralPtr).takeUnretainedValue()
    let request = Unmanaged<CB4UATTRequest>.fromOpaque(requestPtr).takeUnretainedValue()
    
    instance.respond(to: request, withResult: CBATTError.Code(rawValue: Int(result))!)
}

@_cdecl("cb4u_mutable_service_new")
public func cb4u_mutable_service_new(_ serviceUUID: UnsafePointer<CChar>, _ primary: Bool) -> UnsafeMutableRawPointer {
    let service = CBMutableService(type: CBUUID(string: String(cString: serviceUUID)), primary: primary)
    return Unmanaged.passRetained(CB4UMutableService(service: service)).toOpaque()
}

@_cdecl("cb4u_mutable_service_release")
public func cb4u_mutable_service_release(_ servicePtr: UnsafeRawPointer) {
    Unmanaged<CB4UMutableService>.fromOpaque(servicePtr).release()
}

@_cdecl("cb4u_mutable_service_set_characteristics")
public func cb4u_mutable_service_set_characteristics(_ servicePtr: UnsafeRawPointer, _ characteristicsPtr: UnsafePointer<UnsafeRawPointer>?, _ characteristicsCount: Int32) {
    let service = Unmanaged<CB4UMutableService>.fromOpaque(servicePtr).takeUnretainedValue()
    
    if let characteristicsPtr = characteristicsPtr {
        let characteristics = (0..<Int(characteristicsCount)).map { index -> CB4UMutableCharacteristic in
            let characteristicPtr = characteristicsPtr[index]
            return Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).takeUnretainedValue()
        }
        service.setCharacteristics(characteristics)
    } else {
        service.setCharacteristics(nil)
    }
}

@_cdecl("cb4u_mutable_service_clear_characteristics")
public func cb4u_mutable_service_clear_characteristics(_ servicePtr: UnsafeRawPointer) {
    let service = Unmanaged<CB4UMutableService>.fromOpaque(servicePtr).takeUnretainedValue()
    
    service.clearCharacteristics()
}

@_cdecl("cb4u_mutable_service_add_characteristic")
public func cb4u_mutable_service_add_characteristic(_ servicePtr: UnsafeRawPointer, _ characteristicPtr: UnsafeRawPointer) {
    let service = Unmanaged<CB4UMutableService>.fromOpaque(servicePtr).takeUnretainedValue()
    let characteristic = Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).takeUnretainedValue()
    
    service.addCharacteristic(characteristic)
}

@_cdecl("cb4u_mutable_characteristic_new")
public func cb4u_mutable_characteristic_new(
    _ characteristicUUID: UnsafePointer<CChar>,
    _ properties: Int32,
    _ dataBytes: UnsafePointer<UInt8>,
    _ dataLength: Int32,
    _ permissions: Int32
) -> UnsafeMutableRawPointer {
    let data = dataLength > 0 ? Data(bytes: dataBytes, count: Int(dataLength)) : nil
    let characteristic = CBMutableCharacteristic(
        type: CBUUID(string: String(cString: characteristicUUID)),
        properties: CBCharacteristicProperties(rawValue: UInt(properties)),
        value: data,
        permissions: CBAttributePermissions(rawValue: UInt(permissions))
    )
    return Unmanaged.passRetained(CB4UMutableCharacteristic(characteristic: characteristic)).toOpaque()
}

@_cdecl("cb4u_mutable_characteristic_release")
public func cb4u_mutable_characteristic_release(_ characteristicPtr: UnsafeRawPointer) {
    Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).release()
}

@_cdecl("cb4u_mutable_characteristic_value_length")
public func cb4u_mutable_characteristic_value_length(_ characteristicPtr: UnsafeRawPointer) -> Int32 {
    let instance = Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).takeUnretainedValue()
    
    return Int32(instance.valueLength)
}

@_cdecl("cb4u_mutable_characteristic_value")
public func cb4u_mutable_characteristic_value(_ characteristicPtr: UnsafeRawPointer, _ dataBytes: UnsafeMutablePointer<UInt8>, _ dataLength: Int32) -> Int32 {
    let instance = Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).takeUnretainedValue()
    
    guard let value = instance.value else {
        return 0
    }
    
    value.withUnsafeBytes { (valueBytes: UnsafeRawBufferPointer) in
        let valueBytesPtr = valueBytes.bindMemory(to: UInt8.self).baseAddress!
        dataBytes.update(from: valueBytesPtr, count: Int(dataLength))
    }
    return 1
}

@_cdecl("cb4u_mutable_characteristic_set_value")
public func cb4u_mutable_characteristic_set_value(_ characteristicPtr: UnsafeRawPointer, _ dataBytes: UnsafePointer<UInt8>?, _ dataLength: Int32) {
    let instance = Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).takeUnretainedValue()
    
    if let dataBytes = dataBytes {
        instance.value = Data(bytes: dataBytes, count: Int(dataLength))
    } else {
        instance.value = nil
    }
}

@_cdecl("cb4u_mutable_characteristic_properties")
public func cb4u_mutable_characteristic_properties(_ characteristicPtr: UnsafeRawPointer) -> Int32 {
    let instance = Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).takeUnretainedValue()
    
    return Int32(instance.properties.rawValue)
}

@_cdecl("cb4u_mutable_characteristic_set_properties")
public func cb4u_mutable_characteristic_set_properties(_ characteristicPtr: UnsafeRawPointer, _ properties: Int32) {
    let instance = Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).takeUnretainedValue()
    
    instance.properties = CBCharacteristicProperties(rawValue: UInt(properties))
}

@_cdecl("cb4u_mutable_characteristic_permissions")
public func cb4u_mutable_characteristic_permissions(_ characteristicPtr: UnsafeRawPointer) -> Int32 {
    let instance = Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).takeUnretainedValue()
    
    return Int32(instance.permissions.rawValue)
}

@_cdecl("cb4u_mutable_characteristic_set_permissions")
public func cb4u_mutable_characteristic_set_permissions(_ characteristicPtr: UnsafeRawPointer, _ permissions: Int32) {
    let instance = Unmanaged<CB4UMutableCharacteristic>.fromOpaque(characteristicPtr).takeUnretainedValue()
    
    instance.permissions = CBAttributePermissions(rawValue: UInt(permissions))
}

@_cdecl("cb4u_att_request_release")
public func cb4u_att_request_release(_ requestPtr: UnsafeRawPointer) {
    Unmanaged<CB4UATTRequest>.fromOpaque(requestPtr).release()
}

@_cdecl("cb4u_att_request_central")
public func cb4u_att_request_central(_ requestPtr: UnsafeRawPointer) -> UnsafeMutableRawPointer {
    let instance = Unmanaged<CB4UATTRequest>.fromOpaque(requestPtr).takeUnretainedValue()
    
    return Unmanaged.passRetained(instance.central).toOpaque()
}

@_cdecl("cb4u_att_request_characteristic_uuid")
public func cb4u_att_request_characteristic_uuid(
    _ requestPtr: UnsafeRawPointer,
    _ serviceUUID: UnsafeMutablePointer<CChar>,
    _ serviceUUIDSize: Int32,
    _ characteristicUUID: UnsafeMutablePointer<CChar>,
    _ characteristicUUIDSize: Int32
) {
    let instance = Unmanaged<CB4UATTRequest>.fromOpaque(requestPtr).takeUnretainedValue()
    
    if let service = instance.characteristic.service {
        let serviceUUIDString = service.uuid.uuidString
        let serviceUUIDMaxSize = Int(serviceUUIDSize) - 1
        serviceUUIDString.withCString { serviceUUIDStringPtr in
            if serviceUUIDString.count > serviceUUIDMaxSize {
                print("cb4u_att_request_characteristic_uuid: serviceUUIDSize is too small. serviceUUIDSize: \(serviceUUIDSize), serviceUUID.count: \(serviceUUIDString.count)")
            } else {
                strcpy(serviceUUID, serviceUUIDStringPtr)
            }
        }
    }
    let characteristicUUIDString = instance.characteristic.uuid.uuidString
    let characteristicUUIDMaxSize = Int(characteristicUUIDSize) - 1
    
    characteristicUUIDString.withCString { characteristicUUIDStringPtr in
        if characteristicUUIDString.count > characteristicUUIDMaxSize {
            print("cb4u_att_request_characteristic_uuid: characteristicUUIDSize is too small. characteristicUUIDSize: \(characteristicUUIDSize), characteristicUUID.count: \(characteristicUUIDString.count)")
        } else {
            strcpy(characteristicUUID, characteristicUUIDStringPtr)
        }
    }
}

@_cdecl("cb4u_att_request_value_length")
public func cb4u_att_request_value_length(_ requestPtr: UnsafeRawPointer) -> Int32 {
    let instance = Unmanaged<CB4UATTRequest>.fromOpaque(requestPtr).takeUnretainedValue()
    
    return Int32(instance.valueLength)
}

@_cdecl("cb4u_att_request_value")
public func cb4u_att_request_value(_ requestPtr: UnsafeRawPointer, _ dataBytes: UnsafeMutablePointer<UInt8>, _ dataLength: Int32) -> Int32 {
    let instance = Unmanaged<CB4UATTRequest>.fromOpaque(requestPtr).takeUnretainedValue()
    
    guard let value = instance.value else {
        return 0
    }
    
    value.withUnsafeBytes { (valueBytes: UnsafeRawBufferPointer) in
        let valueBytesPtr = valueBytes.bindMemory(to: UInt8.self).baseAddress!
        dataBytes.update(from: valueBytesPtr, count: Int(dataLength))
    }
    return 1
}

@_cdecl("cb4u_att_request_set_value")
public func cb4u_att_request_set_value(_ requestPtr: UnsafeRawPointer, _ dataBytes: UnsafePointer<UInt8>?, _ dataLength: Int32) {
    let instance = Unmanaged<CB4UATTRequest>.fromOpaque(requestPtr).takeUnretainedValue()
    
    if let dataBytes = dataBytes {
        instance.value = Data(bytes: dataBytes, count: Int(dataLength))
    } else {
        instance.value = nil
    }
}

@_cdecl("cb4u_att_request_offset")
public func cb4u_att_request_offset(_ requestPtr: UnsafeRawPointer) -> Int32 {
    let instance = Unmanaged<CB4UATTRequest>.fromOpaque(requestPtr).takeUnretainedValue()
    
    return Int32(instance.offset)
}

@_cdecl("cb4u_att_requests_release")
public func cb4u_att_requests_release(_ requestsPtr: UnsafeRawPointer) {
    Unmanaged<CB4UATTRequests>.fromOpaque(requestsPtr).release()
}

@_cdecl("cb4u_att_requests_count")
public func cb4u_att_requests_count(_ requestsPtr: UnsafeRawPointer) -> Int32 {
    let instance = Unmanaged<CB4UATTRequests>.fromOpaque(requestsPtr).takeUnretainedValue()
    
    return Int32(instance.count)
}

@_cdecl("cb4u_att_requests_request")
public func cb4u_att_requests_request(_ requestsPtr: UnsafeRawPointer, _ index: Int32) -> UnsafeMutableRawPointer {
    let instance = Unmanaged<CB4UATTRequests>.fromOpaque(requestsPtr).takeUnretainedValue()
    
    return Unmanaged.passRetained(instance.request(at: Int(index))).toOpaque()
}
