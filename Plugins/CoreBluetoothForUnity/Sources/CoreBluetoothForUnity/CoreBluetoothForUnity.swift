import CoreBluetooth

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
public typealias CB4UCentralManagerDidDiscoverPeripheralHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UCentralManagerDidUpdateStateHandler = @convention(c) (UnsafeRawPointer, Int32) -> Void

public typealias CB4UPeripheralDidDiscoverServicesHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UPeripheralDidDiscoverCharacteristicsHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, UnsafePointer<CChar>, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UPeripheralDidUpdateValueForCharacteristicHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, UnsafePointer<CChar>, UnsafePointer<CChar>, UnsafePointer<UInt8>, Int32, Int32) -> Void

@_cdecl("cb4u_central_manager_register_handlers")
public func cb4u_central_manager_register_handlers(
    _ centralPtr: UnsafeRawPointer,
    _ didConnectHandler: @escaping CB4UCentralManagerDidConnectHandler,
    _ didDisconnectPeripheralHandler: @escaping CB4UCentralManagerDidDisconnectPeripheralHandler,
    _ didFailToConnectHandler: @escaping CB4UCentralManagerDidFailToConnectHandler,
    _ didDiscoverPeripheralHandler: @escaping CB4UCentralManagerDidDiscoverPeripheralHandler,
    _ didUpdateStateHandler: @escaping CB4UCentralManagerDidUpdateStateHandler,
    _ peripheralDidDiscoverServicesHandler: @escaping CB4UPeripheralDidDiscoverServicesHandler,
    _ peripheralDidDiscoverCharacteristicsHandler: @escaping CB4UPeripheralDidDiscoverCharacteristicsHandler,
    _ peripheralDidUpdateValueForCharacteristicHandler: @escaping CB4UPeripheralDidUpdateValueForCharacteristicHandler
) {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    instance.didConnectHandler = didConnectHandler
    instance.didDisconnectPeripheralHandler = didDisconnectPeripheralHandler
    instance.didFailToConnectHandler = didFailToConnectHandler
    instance.didDiscoverPeripheralHandler = didDiscoverPeripheralHandler
    instance.didUpdateStateHandler = didUpdateStateHandler
    
    instance.peripheralDidDiscoverServicesHandler = peripheralDidDiscoverServicesHandler
    instance.peripheralDidDiscoverCharacteristicsHandler = peripheralDidDiscoverCharacteristicsHandler
    instance.peripheralDidUpdateValueForCharacteristicHandler = peripheralDidUpdateValueForCharacteristicHandler
}

@_cdecl("cb4u_central_manager_connect")
public func cb4u_central_manager_connect(_ centralPtr: UnsafeRawPointer, _ peripheralId: UnsafePointer<CChar>) -> Int32 {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    return instance.connect(peripheralId: String(cString: peripheralId))
}

@_cdecl("cb4u_central_manager_cancel_peripheral_connection")
public func cb4u_central_manager_cancel_peripheral_connection(_ centralPtr: UnsafeRawPointer, _ peripheralId: UnsafePointer<CChar>) -> Int32 {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    return instance.cancelPeripheralConnection(peripheralId: String(cString: peripheralId))
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

@_cdecl("cb4u_central_manager_peripheral_discover_services")
public func cb4u_central_manager_peripheral_discover_services(
    _ centralPtr: UnsafeRawPointer,
    _ peripheralId: UnsafePointer<CChar>,
    _ serviceUUIDs: UnsafePointer<UnsafePointer<CChar>?>,
    _ serviceUUIDsCount: Int32
) -> Int32 {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    let serviceUUIDsArray = (0..<Int(serviceUUIDsCount)).map { index -> CBUUID in
        let uuidString = String(cString: serviceUUIDs[index]!)
        return CBUUID(string: uuidString)
    }
    
    return instance.peripheralDiscoverServices(String(cString: peripheralId), serviceUUIDsArray)
}

@_cdecl("cb4u_central_manager_peripheral_discover_characteristics")
public func cb4u_central_manager_peripheral_discover_characteristics(
    _ centralPtr: UnsafeRawPointer,
    _ peripheralId: UnsafePointer<CChar>,
    _ serviceUUID: UnsafePointer<CChar>,
    _ characteristicUUIDs: UnsafePointer<UnsafePointer<CChar>?>,
    _ characteristicUUIDsCount: Int32
) -> Int32 {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    let characteristicUUIDsArray = (0..<Int(characteristicUUIDsCount)).map { index -> CBUUID in
        let uuidString = String(cString: characteristicUUIDs[index]!)
        return CBUUID(string: uuidString)
    }
    
    return instance.peripheralDiscoverCharacteristics(String(cString: peripheralId), CBUUID(string: String(cString: serviceUUID)), characteristicUUIDsArray)
}

@_cdecl("cb4u_central_manager_peripheral_read_characteristic_value")
public func cb4u_central_manager_peripheral_read_characteristic_value(
    _ centralPtr: UnsafeRawPointer,
    _ peripheralId: UnsafePointer<CChar>,
    _ serviceUUID: UnsafePointer<CChar>,
    _ characteristicUUID: UnsafePointer<CChar>
) -> Int32 {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    return instance.peripheralReadCharacteristicValue(String(cString: peripheralId), CBUUID(string: String(cString: serviceUUID)), CBUUID(string: String(cString: characteristicUUID)))
}

@_cdecl("cb4u_central_manager_peripheral_state")
public func cb4u_central_manager_peripheral_state(_ centralPtr: UnsafeRawPointer, _ peripheralId: UnsafePointer<CChar>) -> Int32 {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    return instance.peripheralState(String(cString: peripheralId))
}

@_cdecl("cb4u_central_manager_characteristic_properties")
public func cb4u_central_manager_characteristic_properties(_ centralPtr: UnsafeRawPointer, _ peripheralId: UnsafePointer<CChar>, _ serviceUUID: UnsafePointer<CChar>, _ characteristicUUID: UnsafePointer<CChar>) -> Int32 {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    return instance.characteristicProperties(String(cString: peripheralId), CBUUID(string: String(cString: serviceUUID)), CBUUID(string: String(cString: characteristicUUID)))
}

