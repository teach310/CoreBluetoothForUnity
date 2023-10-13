include .env

BUILD_DIR := Build
BASE_NAME := CoreBluetoothForUnity
WORKSPACE := $(BASE_NAME).xcworkspace
DYLIB_NAME := lib$(BASE_NAME)

.PHONY: clean
clean:
	rm -rf $(BUILD_DIR)
	rm -rf .build

.PHONY: dylib/build
dylib/build:
	swift build -c release --arch arm64 --arch x86_64

.PHONY: dylib/cp
dylib/cp:
	cp -f .build/apple/Products/Release/$(DYLIB_NAME).dylib $(UNITY_PLUGIN_MACOS_DIR)

.PHONY: dylib
dylib: clean dylib/build dylib/cp

.PHONY: framework/build
framework/build:
	mint run swift-create-xcframework --output ./$(BUILD_DIR)/Framework --platform ios --configuration release

.PHONY: framework/cp
framework/cp:
	rm -rf $(UNITY_PLUGIN_IOS_DIR)/$(BASE_NAME).framework
	cp -r ./$(BUILD_DIR)/Framework/$(BASE_NAME).xcframework/ios-arm64/$(BASE_NAME).framework $(UNITY_PLUGIN_IOS_DIR)/$(BASE_NAME).framework

.PHONY: framework
framework: clean framework/build framework/cp
