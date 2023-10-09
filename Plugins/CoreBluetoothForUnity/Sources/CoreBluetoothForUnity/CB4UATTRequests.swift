import CoreBluetooth

public class CB4UATTRequests {
    var requests: [CBATTRequest] = []
    
    public init(requests: [CBATTRequest]) {
        self.requests = requests
    }
    
    public var count: Int {
        return requests.count
    }
    
    public func request(at index: Int) -> CB4UATTRequest {
        return CB4UATTRequest(request: requests[index])
    }
}
