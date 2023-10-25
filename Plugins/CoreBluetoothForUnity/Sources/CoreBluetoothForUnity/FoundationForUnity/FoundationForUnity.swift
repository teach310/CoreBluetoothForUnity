import Foundation

@_cdecl("ns_object_release")
public func ns_object_release(_ handle: UnsafeRawPointer) {
    Unmanaged<NSObject>.fromOpaque(handle).release()
}

@_cdecl("ns_number_new_bool")
public func ns_number_new_bool(_ value: Bool) -> UnsafeMutableRawPointer {
    let instance = NSNumber(value: value)
    return Unmanaged.passRetained(instance).toOpaque()
}

@_cdecl("ns_number_new_int")
public func ns_number_new_int(_ value: Int32) -> UnsafeMutableRawPointer {
    let instance = NSNumber(value: value)
    return Unmanaged.passRetained(instance).toOpaque()
}

@_cdecl("ns_number_bool_value")
public func ns_number_bool_value(_ handle: UnsafeRawPointer) -> Bool {
    let instance = Unmanaged<NSNumber>.fromOpaque(handle).takeUnretainedValue()
    return instance.boolValue
}

@_cdecl("ns_number_int_value")
public func ns_number_int_value(_ handle: UnsafeRawPointer) -> Int32 {
    let instance = Unmanaged<NSNumber>.fromOpaque(handle).takeUnretainedValue()
    return instance.int32Value
}
