@_cdecl("cb4u_central_manager_new")
public func cb4u_central_manager_new() -> UnsafeMutableRawPointer {
    return Unmanaged.passRetained(CB4UCentralManager()).toOpaque()
}

@_cdecl("cb4u_central_manager_release")
public func cb4u_central_manager_release(_ centralManagerPtr: UnsafeRawPointer) {
    Unmanaged<CB4UCentralManager>.fromOpaque(centralManagerPtr).release()
}
