# AvRouting API

| Property Name | Parent Path | Version | Description | Type | Methods | Validation Rules |
|---|---|---|---|---|---|---|
| AvRouting | /Device/ | 2.0.0 | The object containing the AV routing configuration. | Object | N/A | N/A |
| RouteControl | /Device/AvRouting/ | 2.1.0 | The object containing the route control options. | Object | N/A | N/A |
| IsLayer3Enabled | /Device/AvRouting/RouteControl/ | 2.1.0 | Indicates whether the network layer is enabled. true = enabled, false = disabled | Boolean | GET, POST | N/A |
| IsUsbFollowsVideoEnabled | /Device/AvRouting/RouteControl/ | 2.1.0 | Indicates whether USB follows video is enabled. true = enabled, false = disabled | Boolean | GET, POST | N/A |
| IsChangeUsbRemoteDeviceEnabled | /Device/AvRouting/RouteControl/ | 2.1.0 | Indicates whether changing the USB remote device is enabled. true = enabled, false = disabled | Boolean | GET, POST | N/A |
| IsSecondaryAudioFollowsVideoEnabled | /Device/AvRouting/RouteControl/ | 2.1.0 | Indicates whether secondary audio follows video is enabled. true = enabled, false = disabled | Boolean | GET, POST | N/A |
| Routes | /Device/AvRouting/ | 2.0.0 | Contains the routing objects supported on the device. | Object Collection | GET, POST | N/A |
| Name | /Device/AvRouting/Routes/x/ | 2.0.0 | The name of the stream object. | String | GET | N/A |
| AudioSource | /Device/AvRouting/Routes/x/ | 2.0.0 | The unique identifier for the desired audio source. | String | GET, POST | UUID (Universally Unique Identifier) |
| VideoSource | /Device/AvRouting/Routes/x/ | 2.0.0 | The unique identifier for the desired video source. | String | GET, POST | UUID (Universally Unique Identifier) |
| UsbSource | /Device/AvRouting/Routes/x/ | 2.1.0 | The unique identifier for the desired USB source. | String | GET, POST | UUID (Universally Unique Identifier) |
| AutomaticStreamRoutingEnabled | /Device/AvRouting/Routes/x/ | 2.0.0 | Indicates whether automatic stream routing is enabled. true = enabled, false = disabled | Boolean | GET, POST | N/A |
| UniqueId | /Device/AvRouting/Routes/x/ | 2.0.0 | The unique ID of the stream object. | String | GET | UUID (Universally Unique Identifier) |
| Version | /Device/AvRouting/ | 2.1.0 | "2.0.0": Initial public release, "2.1.0": Added RouteControl, UsbSource, and SecondaryAudioSource. | String | GET | N/A |
