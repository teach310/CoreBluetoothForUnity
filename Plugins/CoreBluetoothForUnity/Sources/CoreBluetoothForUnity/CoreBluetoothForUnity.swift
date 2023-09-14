import CoreBluetooth

@_cdecl("cb4u_central_manager_new")
public func cb4u_central_manager_new() -> UnsafeMutableRawPointer {
    return Unmanaged.passRetained(CB4UCentralManager()).toOpaque()
}

@_cdecl("cb4u_central_manager_release")
public func cb4u_central_manager_release(_ centralManagerPtr: UnsafeRawPointer) {
    Unmanaged<CB4UCentralManager>.fromOpaque(centralManagerPtr).release()
}

public typealias CB4UCentralManagerDidDiscoverPeripheralHandler = @convention(c) (UnsafeRawPointer, UnsafePointer<CChar>, UnsafePointer<CChar>, Int32) -> Void
public typealias CB4UCentralManagerDidUpdateStateHandler = @convention(c) (UnsafeRawPointer, Int32) -> Void

@_cdecl("cb4u_central_manager_register_handlers")
public func cb4u_central_manager_register_handlers(
    _ centralPtr: UnsafeRawPointer,
    _ didDiscoverPeripheralHandler: @escaping CB4UCentralManagerDidDiscoverPeripheralHandler,
    _ didUpdateStateHandler: @escaping CB4UCentralManagerDidUpdateStateHandler
) {
    let instance = Unmanaged<CB4UCentralManager>.fromOpaque(centralPtr).takeUnretainedValue()
    
    instance.didDiscoverPeripheralHandler = didDiscoverPeripheralHandler
    instance.didUpdateStateHandler = didUpdateStateHandler
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
