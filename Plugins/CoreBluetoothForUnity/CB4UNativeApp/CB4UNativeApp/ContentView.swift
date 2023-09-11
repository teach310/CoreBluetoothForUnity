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
        VStack {
            Text("HOME")
                .font(.largeTitle)
                .fontWeight(.bold)
                .foregroundColor(.accentColor)
            
            
            Button(action: {
//                print(cb4u_hello())
            }) {
                Text("Tap me!")
                    .font(.title)
                    .fontWeight(.bold)
                    .foregroundColor(.white)
                    .padding()
                    .background(Color.accentColor)
                    .cornerRadius(10)
            }
        }
        .padding()
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}
