#if 0
#elif defined(__arm64__) && __arm64__
// Generated by Apple Swift version 5.8 (swiftlang-5.8.0.124.2 clang-1403.0.22.11.100)
#ifndef COREBLUETOOTHFORUNITY_SWIFT_H
#define COREBLUETOOTHFORUNITY_SWIFT_H
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wgcc-compat"

#if !defined(__has_include)
# define __has_include(x) 0
#endif
#if !defined(__has_attribute)
# define __has_attribute(x) 0
#endif
#if !defined(__has_feature)
# define __has_feature(x) 0
#endif
#if !defined(__has_warning)
# define __has_warning(x) 0
#endif

#if __has_include(<swift/objc-prologue.h>)
# include <swift/objc-prologue.h>
#endif

#pragma clang diagnostic ignored "-Wauto-import"
#if defined(__OBJC__)
#include <Foundation/Foundation.h>
#endif
#if defined(__cplusplus)
#include <cstdint>
#include <cstddef>
#include <cstdbool>
#include <cstring>
#include <stdlib.h>
#include <new>
#include <type_traits>
#else
#include <stdint.h>
#include <stddef.h>
#include <stdbool.h>
#include <string.h>
#endif
#if defined(__cplusplus)
#if __has_include(<ptrauth.h>)
# include <ptrauth.h>
#else
# ifndef __ptrauth_swift_value_witness_function_pointer
#  define __ptrauth_swift_value_witness_function_pointer(x)
# endif
#endif
#endif

#if !defined(SWIFT_TYPEDEFS)
# define SWIFT_TYPEDEFS 1
# if __has_include(<uchar.h>)
#  include <uchar.h>
# elif !defined(__cplusplus)
typedef uint_least16_t char16_t;
typedef uint_least32_t char32_t;
# endif
typedef float swift_float2  __attribute__((__ext_vector_type__(2)));
typedef float swift_float3  __attribute__((__ext_vector_type__(3)));
typedef float swift_float4  __attribute__((__ext_vector_type__(4)));
typedef double swift_double2  __attribute__((__ext_vector_type__(2)));
typedef double swift_double3  __attribute__((__ext_vector_type__(3)));
typedef double swift_double4  __attribute__((__ext_vector_type__(4)));
typedef int swift_int2  __attribute__((__ext_vector_type__(2)));
typedef int swift_int3  __attribute__((__ext_vector_type__(3)));
typedef int swift_int4  __attribute__((__ext_vector_type__(4)));
typedef unsigned int swift_uint2  __attribute__((__ext_vector_type__(2)));
typedef unsigned int swift_uint3  __attribute__((__ext_vector_type__(3)));
typedef unsigned int swift_uint4  __attribute__((__ext_vector_type__(4)));
#endif

#if !defined(SWIFT_PASTE)
# define SWIFT_PASTE_HELPER(x, y) x##y
# define SWIFT_PASTE(x, y) SWIFT_PASTE_HELPER(x, y)
#endif
#if !defined(SWIFT_METATYPE)
# define SWIFT_METATYPE(X) Class
#endif
#if !defined(SWIFT_CLASS_PROPERTY)
# if __has_feature(objc_class_property)
#  define SWIFT_CLASS_PROPERTY(...) __VA_ARGS__
# else
#  define SWIFT_CLASS_PROPERTY(...) 
# endif
#endif
#if !defined(SWIFT_RUNTIME_NAME)
# if __has_attribute(objc_runtime_name)
#  define SWIFT_RUNTIME_NAME(X) __attribute__((objc_runtime_name(X)))
# else
#  define SWIFT_RUNTIME_NAME(X) 
# endif
#endif
#if !defined(SWIFT_COMPILE_NAME)
# if __has_attribute(swift_name)
#  define SWIFT_COMPILE_NAME(X) __attribute__((swift_name(X)))
# else
#  define SWIFT_COMPILE_NAME(X) 
# endif
#endif
#if !defined(SWIFT_METHOD_FAMILY)
# if __has_attribute(objc_method_family)
#  define SWIFT_METHOD_FAMILY(X) __attribute__((objc_method_family(X)))
# else
#  define SWIFT_METHOD_FAMILY(X) 
# endif
#endif
#if !defined(SWIFT_NOESCAPE)
# if __has_attribute(noescape)
#  define SWIFT_NOESCAPE __attribute__((noescape))
# else
#  define SWIFT_NOESCAPE 
# endif
#endif
#if !defined(SWIFT_RELEASES_ARGUMENT)
# if __has_attribute(ns_consumed)
#  define SWIFT_RELEASES_ARGUMENT __attribute__((ns_consumed))
# else
#  define SWIFT_RELEASES_ARGUMENT 
# endif
#endif
#if !defined(SWIFT_WARN_UNUSED_RESULT)
# if __has_attribute(warn_unused_result)
#  define SWIFT_WARN_UNUSED_RESULT __attribute__((warn_unused_result))
# else
#  define SWIFT_WARN_UNUSED_RESULT 
# endif
#endif
#if !defined(SWIFT_NORETURN)
# if __has_attribute(noreturn)
#  define SWIFT_NORETURN __attribute__((noreturn))
# else
#  define SWIFT_NORETURN 
# endif
#endif
#if !defined(SWIFT_CLASS_EXTRA)
# define SWIFT_CLASS_EXTRA 
#endif
#if !defined(SWIFT_PROTOCOL_EXTRA)
# define SWIFT_PROTOCOL_EXTRA 
#endif
#if !defined(SWIFT_ENUM_EXTRA)
# define SWIFT_ENUM_EXTRA 
#endif
#if !defined(SWIFT_CLASS)
# if __has_attribute(objc_subclassing_restricted)
#  define SWIFT_CLASS(SWIFT_NAME) SWIFT_RUNTIME_NAME(SWIFT_NAME) __attribute__((objc_subclassing_restricted)) SWIFT_CLASS_EXTRA
#  define SWIFT_CLASS_NAMED(SWIFT_NAME) __attribute__((objc_subclassing_restricted)) SWIFT_COMPILE_NAME(SWIFT_NAME) SWIFT_CLASS_EXTRA
# else
#  define SWIFT_CLASS(SWIFT_NAME) SWIFT_RUNTIME_NAME(SWIFT_NAME) SWIFT_CLASS_EXTRA
#  define SWIFT_CLASS_NAMED(SWIFT_NAME) SWIFT_COMPILE_NAME(SWIFT_NAME) SWIFT_CLASS_EXTRA
# endif
#endif
#if !defined(SWIFT_RESILIENT_CLASS)
# if __has_attribute(objc_class_stub)
#  define SWIFT_RESILIENT_CLASS(SWIFT_NAME) SWIFT_CLASS(SWIFT_NAME) __attribute__((objc_class_stub))
#  define SWIFT_RESILIENT_CLASS_NAMED(SWIFT_NAME) __attribute__((objc_class_stub)) SWIFT_CLASS_NAMED(SWIFT_NAME)
# else
#  define SWIFT_RESILIENT_CLASS(SWIFT_NAME) SWIFT_CLASS(SWIFT_NAME)
#  define SWIFT_RESILIENT_CLASS_NAMED(SWIFT_NAME) SWIFT_CLASS_NAMED(SWIFT_NAME)
# endif
#endif
#if !defined(SWIFT_PROTOCOL)
# define SWIFT_PROTOCOL(SWIFT_NAME) SWIFT_RUNTIME_NAME(SWIFT_NAME) SWIFT_PROTOCOL_EXTRA
# define SWIFT_PROTOCOL_NAMED(SWIFT_NAME) SWIFT_COMPILE_NAME(SWIFT_NAME) SWIFT_PROTOCOL_EXTRA
#endif
#if !defined(SWIFT_EXTENSION)
# define SWIFT_EXTENSION(M) SWIFT_PASTE(M##_Swift_, __LINE__)
#endif
#if !defined(OBJC_DESIGNATED_INITIALIZER)
# if __has_attribute(objc_designated_initializer)
#  define OBJC_DESIGNATED_INITIALIZER __attribute__((objc_designated_initializer))
# else
#  define OBJC_DESIGNATED_INITIALIZER 
# endif
#endif
#if !defined(SWIFT_ENUM_ATTR)
# if __has_attribute(enum_extensibility)
#  define SWIFT_ENUM_ATTR(_extensibility) __attribute__((enum_extensibility(_extensibility)))
# else
#  define SWIFT_ENUM_ATTR(_extensibility) 
# endif
#endif
#if !defined(SWIFT_ENUM)
# define SWIFT_ENUM(_type, _name, _extensibility) enum _name : _type _name; enum SWIFT_ENUM_ATTR(_extensibility) SWIFT_ENUM_EXTRA _name : _type
# if __has_feature(generalized_swift_name)
#  define SWIFT_ENUM_NAMED(_type, _name, SWIFT_NAME, _extensibility) enum _name : _type _name SWIFT_COMPILE_NAME(SWIFT_NAME); enum SWIFT_COMPILE_NAME(SWIFT_NAME) SWIFT_ENUM_ATTR(_extensibility) SWIFT_ENUM_EXTRA _name : _type
# else
#  define SWIFT_ENUM_NAMED(_type, _name, SWIFT_NAME, _extensibility) SWIFT_ENUM(_type, _name, _extensibility)
# endif
#endif
#if !defined(SWIFT_UNAVAILABLE)
# define SWIFT_UNAVAILABLE __attribute__((unavailable))
#endif
#if !defined(SWIFT_UNAVAILABLE_MSG)
# define SWIFT_UNAVAILABLE_MSG(msg) __attribute__((unavailable(msg)))
#endif
#if !defined(SWIFT_AVAILABILITY)
# define SWIFT_AVAILABILITY(plat, ...) __attribute__((availability(plat, __VA_ARGS__)))
#endif
#if !defined(SWIFT_WEAK_IMPORT)
# define SWIFT_WEAK_IMPORT __attribute__((weak_import))
#endif
#if !defined(SWIFT_DEPRECATED)
# define SWIFT_DEPRECATED __attribute__((deprecated))
#endif
#if !defined(SWIFT_DEPRECATED_MSG)
# define SWIFT_DEPRECATED_MSG(...) __attribute__((deprecated(__VA_ARGS__)))
#endif
#if !defined(SWIFT_DEPRECATED_OBJC)
# if __has_feature(attribute_diagnose_if_objc)
#  define SWIFT_DEPRECATED_OBJC(Msg) __attribute__((diagnose_if(1, Msg, "warning")))
# else
#  define SWIFT_DEPRECATED_OBJC(Msg) SWIFT_DEPRECATED_MSG(Msg)
# endif
#endif
#if defined(__OBJC__)
#if !defined(IBSegueAction)
# define IBSegueAction 
#endif
#endif
#if !defined(SWIFT_EXTERN)
# if defined(__cplusplus)
#  define SWIFT_EXTERN extern "C"
# else
#  define SWIFT_EXTERN extern
# endif
#endif
#if !defined(SWIFT_CALL)
# define SWIFT_CALL __attribute__((swiftcall))
#endif
#if !defined(SWIFT_INDIRECT_RESULT)
# define SWIFT_INDIRECT_RESULT __attribute__((swift_indirect_result))
#endif
#if !defined(SWIFT_CONTEXT)
# define SWIFT_CONTEXT __attribute__((swift_context))
#endif
#if !defined(SWIFT_ERROR_RESULT)
# define SWIFT_ERROR_RESULT __attribute__((swift_error_result))
#endif
#if defined(__cplusplus)
# define SWIFT_NOEXCEPT noexcept
#else
# define SWIFT_NOEXCEPT 
#endif
#if defined(_WIN32)
#if !defined(SWIFT_IMPORT_STDLIB_SYMBOL)
# define SWIFT_IMPORT_STDLIB_SYMBOL __declspec(dllimport)
#endif
#else
#if !defined(SWIFT_IMPORT_STDLIB_SYMBOL)
# define SWIFT_IMPORT_STDLIB_SYMBOL 
#endif
#endif
#if defined(__OBJC__)
#if __has_feature(objc_modules)
#if __has_warning("-Watimport-in-framework-header")
#pragma clang diagnostic ignored "-Watimport-in-framework-header"
#endif
@import CoreBluetooth;
@import Foundation;
@import ObjectiveC;
#endif

#endif
#pragma clang diagnostic ignored "-Wproperty-attribute-mismatch"
#pragma clang diagnostic ignored "-Wduplicate-method-arg"
#if __has_warning("-Wpragma-clang-attribute")
# pragma clang diagnostic ignored "-Wpragma-clang-attribute"
#endif
#pragma clang diagnostic ignored "-Wunknown-pragmas"
#pragma clang diagnostic ignored "-Wnullability"
#pragma clang diagnostic ignored "-Wdollar-in-identifier-extension"

#if __has_attribute(external_source_symbol)
# pragma push_macro("any")
# undef any
# pragma clang attribute push(__attribute__((external_source_symbol(language="Swift", defined_in="CoreBluetoothForUnity",generated_declaration))), apply_to=any(function,enum,objc_interface,objc_category,objc_protocol))
# pragma pop_macro("any")
#endif

#if defined(__OBJC__)

SWIFT_CLASS("_TtC21CoreBluetoothForUnity18CB4UCentralManager")
@interface CB4UCentralManager : NSObject
- (nonnull instancetype)init OBJC_DESIGNATED_INITIALIZER;
@end


@class CBCentralManager;
@class CBPeripheral;
@class NSString;
@class NSNumber;

@interface CB4UCentralManager (SWIFT_EXTENSION(CoreBluetoothForUnity)) <CBCentralManagerDelegate>
- (void)centralManager:(CBCentralManager * _Nonnull)central didConnectPeripheral:(CBPeripheral * _Nonnull)peripheral;
- (void)centralManager:(CBCentralManager * _Nonnull)central didDisconnectPeripheral:(CBPeripheral * _Nonnull)peripheral error:(NSError * _Nullable)error;
- (void)centralManager:(CBCentralManager * _Nonnull)central didFailToConnectPeripheral:(CBPeripheral * _Nonnull)peripheral error:(NSError * _Nullable)error;
- (void)centralManager:(CBCentralManager * _Nonnull)central didDiscoverPeripheral:(CBPeripheral * _Nonnull)peripheral advertisementData:(NSDictionary<NSString *, id> * _Nonnull)advertisementData RSSI:(NSNumber * _Nonnull)RSSI;
- (void)centralManagerDidUpdateState:(CBCentralManager * _Nonnull)central;
@end

@class CBService;
@class CBCharacteristic;

@interface CB4UCentralManager (SWIFT_EXTENSION(CoreBluetoothForUnity)) <CBPeripheralDelegate>
- (void)peripheral:(CBPeripheral * _Nonnull)peripheral didDiscoverServices:(NSError * _Nullable)error;
- (void)peripheral:(CBPeripheral * _Nonnull)peripheral didDiscoverCharacteristicsForService:(CBService * _Nonnull)service error:(NSError * _Nullable)error;
- (void)peripheral:(CBPeripheral * _Nonnull)peripheral didUpdateValueForCharacteristic:(CBCharacteristic * _Nonnull)characteristic error:(NSError * _Nullable)error;
- (void)peripheral:(CBPeripheral * _Nonnull)peripheral didWriteValueForCharacteristic:(CBCharacteristic * _Nonnull)characteristic error:(NSError * _Nullable)error;
- (void)peripheral:(CBPeripheral * _Nonnull)peripheral didUpdateNotificationStateForCharacteristic:(CBCharacteristic * _Nonnull)characteristic error:(NSError * _Nullable)error;
@end



SWIFT_CLASS("_TtC21CoreBluetoothForUnity21CB4UPeripheralManager")
@interface CB4UPeripheralManager : NSObject
- (nonnull instancetype)init OBJC_DESIGNATED_INITIALIZER;
@end

@class CBPeripheralManager;

@interface CB4UPeripheralManager (SWIFT_EXTENSION(CoreBluetoothForUnity)) <CBPeripheralManagerDelegate>
- (void)peripheralManagerDidUpdateState:(CBPeripheralManager * _Nonnull)peripheral;
@end


SWIFT_EXTERN int32_t cb4u_central_manager_cancel_peripheral_connection(void const * _Nonnull centralPtr, char const * _Nonnull peripheralId) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN int32_t cb4u_central_manager_characteristic_properties(void const * _Nonnull centralPtr, char const * _Nonnull peripheralId, char const * _Nonnull serviceUUID, char const * _Nonnull characteristicUUID) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN int32_t cb4u_central_manager_connect(void const * _Nonnull centralPtr, char const * _Nonnull peripheralId) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN BOOL cb4u_central_manager_is_scanning(void const * _Nonnull centralPtr) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN void * _Nonnull cb4u_central_manager_new(void) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN int32_t cb4u_central_manager_peripheral_discover_characteristics(void const * _Nonnull centralPtr, char const * _Nonnull peripheralId, char const * _Nonnull serviceUUID, char const * _Nullable const * _Nonnull characteristicUUIDs, int32_t characteristicUUIDsCount) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN int32_t cb4u_central_manager_peripheral_discover_services(void const * _Nonnull centralPtr, char const * _Nonnull peripheralId, char const * _Nullable const * _Nonnull serviceUUIDs, int32_t serviceUUIDsCount) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN int32_t cb4u_central_manager_peripheral_read_characteristic_value(void const * _Nonnull centralPtr, char const * _Nonnull peripheralId, char const * _Nonnull serviceUUID, char const * _Nonnull characteristicUUID) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN int32_t cb4u_central_manager_peripheral_set_notify_value(void const * _Nonnull centralPtr, char const * _Nonnull peripheralId, char const * _Nonnull serviceUUID, char const * _Nonnull characteristicUUID, BOOL enabled) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN int32_t cb4u_central_manager_peripheral_state(void const * _Nonnull centralPtr, char const * _Nonnull peripheralId) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN int32_t cb4u_central_manager_peripheral_write_characteristic_value(void const * _Nonnull centralPtr, char const * _Nonnull peripheralId, char const * _Nonnull serviceUUID, char const * _Nonnull characteristicUUID, uint8_t const * _Nonnull dataBytes, int32_t dataLength, int32_t writeType) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN void cb4u_central_manager_register_handlers(void const * _Nonnull centralPtr, void (* _Nonnull didConnectHandler)(void const * _Nonnull, char const * _Nonnull), void (* _Nonnull didDisconnectPeripheralHandler)(void const * _Nonnull, char const * _Nonnull, int32_t), void (* _Nonnull didFailToConnectHandler)(void const * _Nonnull, char const * _Nonnull, int32_t), void (* _Nonnull didDiscoverPeripheralHandler)(void const * _Nonnull, char const * _Nonnull, char const * _Nonnull, int32_t), void (* _Nonnull didUpdateStateHandler)(void const * _Nonnull, int32_t), void (* _Nonnull peripheralDidDiscoverServicesHandler)(void const * _Nonnull, char const * _Nonnull, char const * _Nonnull, int32_t), void (* _Nonnull peripheralDidDiscoverCharacteristicsHandler)(void const * _Nonnull, char const * _Nonnull, char const * _Nonnull, char const * _Nonnull, int32_t), void (* _Nonnull peripheralDidUpdateValueForCharacteristicHandler)(void const * _Nonnull, char const * _Nonnull, char const * _Nonnull, char const * _Nonnull, uint8_t const * _Nonnull, int32_t, int32_t), void (* _Nonnull peripheralDidWriteValueForCharacteristicHandler)(void const * _Nonnull, char const * _Nonnull, char const * _Nonnull, char const * _Nonnull, int32_t), void (* _Nonnull peripheralDidUpdateNotificationStateForCharacteristicHandler)(void const * _Nonnull, char const * _Nonnull, char const * _Nonnull, char const * _Nonnull, int32_t, int32_t));


SWIFT_EXTERN void cb4u_central_manager_release(void const * _Nonnull centralManagerPtr);


SWIFT_EXTERN void cb4u_central_manager_scan_for_peripherals(void const * _Nonnull centralPtr, char const * _Nullable const * _Nonnull serviceUUIDs, int32_t serviceUUIDsCount);


SWIFT_EXTERN void cb4u_central_manager_stop_scan(void const * _Nonnull centralPtr);


SWIFT_EXTERN void * _Nonnull cb4u_mutable_characteristic_new(char const * _Nonnull characteristicUUID, int32_t properties, uint8_t const * _Nonnull dataBytes, int32_t dataLength, int32_t permissions) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN void cb4u_mutable_characteristic_release(void const * _Nonnull characteristicPtr);


SWIFT_EXTERN void * _Nonnull cb4u_peripheral_manager_new(void) SWIFT_WARN_UNUSED_RESULT;


SWIFT_EXTERN void cb4u_peripheral_manager_register_handlers(void const * _Nonnull peripheralManagerPtr, void (* _Nonnull didUpdateStateHandler)(void const * _Nonnull, int32_t));


SWIFT_EXTERN void cb4u_peripheral_manager_release(void const * _Nonnull peripheralManagerPtr);

#endif
#if defined(__cplusplus)
#endif
#if __has_attribute(external_source_symbol)
# pragma clang attribute pop
#endif
#pragma clang diagnostic pop
#endif

#else
#error unsupported Swift architecture
#endif
