BUILD_DIR := Build
BASE_NAME := CoreBluetoothForUnity
WORKSPACE := $(BASE_NAME).xcworkspace
BUNDLE_NAME := CB4UBundle

.PHONY: clean
clean:
	rm -rf $(BUILD_DIR)
	rm -rf .build

.PHONY: bundle/build
bundle/build:
	xcodebuild -workspace $(WORKSPACE) -scheme $(BUNDLE_NAME) -destination 'platform=macOS,arch=arm64' -configuration Release -derivedDataPath ./$(BUILD_DIR)/Bundle build

.PHONY: framework/build
framework/build:
	mint run swift-create-xcframework --output ./$(BUILD_DIR)/Framework --platform ios --configuration release
