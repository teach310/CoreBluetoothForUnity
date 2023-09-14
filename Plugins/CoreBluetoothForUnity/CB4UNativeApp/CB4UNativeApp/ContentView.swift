//
//  ContentView.swift
//  CB4UNativeApp
//
//  Created by Taichi Sato on 2023/09/06.
//

import SwiftUI
import CoreBluetoothForUnity

struct ContentView: View {
    var body: some View {
        NavigationView {
            VStack {
                Text("CB4U")
                    .font(.largeTitle)
                    .fontWeight(.bold)
                    .foregroundColor(.accentColor)
                
                NavigationLink(destination: BLEPeripheralView()) {
                    Text("BLEPeripheral")
                        .font(.title)
                        .fontWeight(.bold)
                        .foregroundColor(.white)
                        .padding()
                        .background(Color.blue)
                        .cornerRadius(10)
                }
            }
        }
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}
