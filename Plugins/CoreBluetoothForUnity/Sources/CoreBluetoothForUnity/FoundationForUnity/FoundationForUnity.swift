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

@_cdecl("ns_string_new")
public func ns_string_new(_ str: UnsafePointer<CChar>) -> UnsafeMutableRawPointer {
    let nsstring = NSString(utf8String: str)!
    return Unmanaged.passRetained(nsstring).toOpaque()
}

@_cdecl("ns_string_length_of_bytes_utf8")
public func ns_string_length_of_bytes_utf8(_ handle: UnsafeRawPointer) -> Int32 {
    let nsstring = Unmanaged<NSString>.fromOpaque(handle).takeUnretainedValue()
    return Int32(nsstring.lengthOfBytes(using: String.Encoding.utf8.rawValue))
}

@_cdecl("ns_string_get_cstring_and_length")
public func ns_string_get_cstring_and_length(_ handle: UnsafeRawPointer, _ ptr: UnsafeMutablePointer<UnsafePointer<CChar>?>, _ length: UnsafeMutablePointer<Int32>) {
    let nsstring = Unmanaged<NSString>.fromOpaque(handle).takeUnretainedValue()
    if let cstring = nsstring.utf8String {
        ptr.pointee = UnsafePointer(cstring)
        length.pointee = Int32(strlen(cstring))
    } else {
        ptr.pointee = nil
        length.pointee = 0
    }
}

@_cdecl("ns_mutable_dictionary_new")
public func ns_mutable_dictionary_new() -> UnsafeMutableRawPointer {
    let instance = NSMutableDictionary()
    return Unmanaged.passRetained(instance).toOpaque()
}

@_cdecl("ns_mutable_dictionary_get_value")
public func ns_mutable_dictionary_get_value(_ handle: UnsafeRawPointer, _ keyPtr: UnsafeRawPointer) -> UnsafeMutableRawPointer? {
    let instance = Unmanaged<NSMutableDictionary>.fromOpaque(handle).takeUnretainedValue()

    let key = Unmanaged<NSObject>.fromOpaque(keyPtr).takeUnretainedValue()
    if let value = instance[key] as? NSObject {
        return Unmanaged.passRetained(value).toOpaque()
    }

    return nil
}

@_cdecl("ns_mutable_dictionary_set_value")
public func ns_mutable_dictionary_set_value(_ handle: UnsafeRawPointer, _ keyPtr: UnsafeRawPointer, _ valuePtr: UnsafeRawPointer?) {
    let instance = Unmanaged<NSMutableDictionary>.fromOpaque(handle).takeUnretainedValue()

    let key = Unmanaged<NSObject>.fromOpaque(keyPtr).takeUnretainedValue()
    if let valuePtr = valuePtr {
        let value = Unmanaged<NSObject>.fromOpaque(valuePtr).takeUnretainedValue()
        instance[key] = value
    } else {
        instance[key] = nil
    }
}
