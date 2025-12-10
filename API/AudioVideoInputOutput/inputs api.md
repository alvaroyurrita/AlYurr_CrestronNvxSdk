# AudioVideoInputOutput Inputs API

| Property Name | Parent Path | Version | Description | Type | Methods | Validation Rules |
|---|---|---|---|---|---|---|
| Inputs | /Device/AudioVideoInputOutput/ | 2.0.0 | Contains the inputs supported on the device. | Object collection | GET, POST | N/A |
| Name | /Device/AudioVideoInputOutput/Inputs/x/ | 2.0.0 | The input name. | String | GET | N/A |
| Uuid | /Device/AudioVideoInputOutput/Inputs/x/ | 2.0.0 | The unique ID for the input. | String | GET | N/A |
| EndpointExists | /Device/AudioVideoInputOutput/Inputs/x/ | 2.0.2 | Indicates whether an endpoint support exists on this input (true) or not (false). | Boolean | GET | N/A |
| EndpointId | /Device/AudioVideoInputOutput/Inputs/x/ | 2.3.0 | The static endpoint ID (such as "Input1"). | String | GET | "Input1", "Input2", [...] "InputN" |
| VideoPortTypeSelect | /Device/AudioVideoInputOutput/Inputs/x/ | 2.0.0 | The video type selected for the port. | String | GET, POST | "Hdmi", "Vga", "Bnc", "Auto" |
| Ports | /Device/AudioVideoInputOutput/Inputs/x/ | 2.0.0 | Contains supported physical ports for a particular input. | Object Collection | GET, POST | N/A |
| Power | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.3.0 | Contains endpoint power-related control properties. | Object Collection | GET, POST | N/A |
| DmInputType | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Power/ | 2.3.0 | The type of endpoint power control that is applied. | String | GET, POST | "OFF", "Dm", "DmLite" |
| PortType | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The physical port type. | String | GET | "Hdmi", "Analog","Audio" |
| Uuid | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The unique ID for the port. | String | GET | N/A |
| IsSyncDetected | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | Indicates that video sync levels are detected at the HDMI input. | Boolean | GET | N/A |
| IsSourceDetected | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | Indicates that a video signal is detected at the HDMI input. | Boolean | GET | N/A |
| IsInterlacedDetected | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | Indicates whether interlaced video is detected. true = detected, false = not detected | Boolean | GET | N/A |
| ColorSpace | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The color space of the input. | String | GET | N/A |
| ColorDepth | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The color depth of the input. | String | GET | N/A |
| HorizontalResolution | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The horizontal resolution detected on the input. | Numeric (Unsigned 16-bit) | GET | N/A |
| VerticalResolution | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The vertical resolution detected on the input. | Numeric (Unsigned 16-bit) | GET | N/A |
| FramesPerSecond | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The FPS (frames per second) detected on the input. | Numeric (Unsigned 16-bit) | GET | N/A |
| AspectRatio | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The aspect ratio detected on the input. | String | GET | N/A |
| Edid | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The supported EDIDs for a particular input. | Object Collection | GET, POST | N/A |
| CurrentEdid | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Edid/ | 2.0.0 | The selected EDID name. | String | GET | N/A |
| EdidList | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Edid/ | 2.0.0 | The collection of supported EDIDs. | Object Collection | GET, POST | N/A |
| Name | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Edid/EdidList/x/ | 2.0.0 | The name of the selected EDID. | String | GET | N/A |
| Type | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Edid/EdidList/x/ | 2.0.0 | The EDID type. | String | GET | N/A |
| UploadEdid | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Edid/ | 2.0.0 | Used to upload a new EDID. | String | POST | N/A |
| ApplyEdid | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Edid/ | 2.0.0 | Used to apply an EDID. | Object | POST | N/A |
| DeleteEdid | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Edid/ | 2.0.0 | Used to delete an EDID. | Object | POST | N/A |
| Hdmi | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The HDMI video object. | Object | GET, POST | N/A |
| IsSourceHdcpActive | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Hdmi/ | 2.0.0 | If true, indicates HDCP (High‑Bandwidth Digital Content Protection) is present on the input. | Boolean | GET | N/A |
| HdcpReceiverCapability | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Hdmi/ | 2.0.0 | Sets whether the HDCP receiver capability is disabled or enabled. | String | GET, POST | N/A |
| HdcpState | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Hdmi/ | 2.0.0 | The current HDCP state. | String | GET | N/A |
| IsCecErrorDetected | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Hdmi/ | 2.0.0 | Indicates whether a CEC NACK (No ACK) error has occurred. This error occurs when the connected CEC device fails to respond to a CEC command (with an ACK pulse). The state remains true until the CEC command is sent successfully. | Boolean | GET | N/A |
| Status3D | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Hdmi/ | 2.0.0 | Indicates whether incoming video is 2D or 3D. 0d = 2D, 1d = 3D | Numeric (Unsigned 16-bit) | GET | N/A |
| ReceiveCecMessage | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Hdmi/ | 2.0.0 | The serial data received via CEC. | String | GET | N/A |
| TransmitCecMessage | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Hdmi/ | 2.0.0 | Used to transmit serial data via CEC. | String | POST | N/A |
| UnsupportedVideoDetected | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Hdmi/ | 2.4.0 | Indicates whether an unsupported video type is detected (true) or not (false). | Boolean | GET | N/A |
| Audio | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/ | 2.0.0 | The audio object. | Object | GET, POST | N/A |
| AudioTypeSelect | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Audio/ | 2.0.0 | Selects the audio input to use for devices that support multiple audio inputs: 0d = Auto, 1d = HDMI, 2d = Analog, 3d = SPDIF | Numeric (Unsigned 16-bit) | GET, POST | "0 to 3" |
| Digital | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Audio/ | 2.0.0 | The digital audio object | Object | GET | N/A |
| Channels | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Audio/Digital/ | 2.0.0 | The number of audio channels. | Numeric (Unsigned 16-bit) | GET | N/A |
| Format | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Audio/Digital/ | 2.0.0 | The audio format. | String | GET | N/A |
| Version | /Device/AudioVideoInputOutput/ | 2.4.0 | The object version. "2.0.0": Initial public release, "2.0.1": Added HdcpTransmitterMode in Output Section, "2.0.2": Added EndpointExists for Inputs and Outputs, "2.0.3": Added ResolutionOptionsName, ResolutionOptionsVersion for Outputs, "2.1.0": Added Orientation for Outputs, "2.2.0": Added VideoTimeout properties and CecControl object, InputControl subobject, "2.3.0": Added EndpointId, IsEndPointOnline, and DM endpoint power control object support, "2.4.0": Added UnsupportedVideoDetected property in both HDMI input and output | String | GET | N/A |
