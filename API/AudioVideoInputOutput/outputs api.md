# AudioVideoInputOutput Outputs API

| Property Name | Parent Path | Version | Description | Type | Methods | Validation Rules |
|---|---|---|---|---|---|---|
| Outputs | /Device/AudioVideoInputOutput/ | 2.0.0 | Contains the outputs supported on the device. | Object collection | GET, POST | N/A |
| Name | /Device/AudioVideoInputOutput/Outputs/x/ | 2.0.0 | The output name. | String | GET | N/A |
| Uuid | /Device/AudioVideoInputOutput/Outputs/x/ | 2.0.0 | The unique ID for the output. | String | GET | N/A |
| EndpointExists | /Device/AudioVideoInputOutput/Outputs/x/ | 2.0.2 | Indicates whether an endpoint support exists on this output (true) or not (false). | Boolean | GET | N/A |
| VideoPortTypeSelect | /Device/AudioVideoInputOutput/Outputs/x/ | 2.0.0 | The video type selected for the port. | String | GET, POST | "Hdmi", "Vga", "Bnc", "Auto" |
| Ports | /Device/AudioVideoInputOutput/Outputs/x/ | 2.0.0 | Contains supported physical ports for a particular output. | Object Collection | GET, POST | N/A |
| PortType | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The physical port type. | String | GET | "Hdmi", "Analog","Audio" |
| Uuid | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The unique ID for the port. | String | GET | N/A |
| IsSinkConnected | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | Indicates whether sink is connected to the output. | Boolean | GET | N/A |
| IsSourceDetected | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | Indicates whether a source is detected for the output. | Boolean | GET | N/A |
| ColorSpace | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The color space of the output. | String | GET | N/A |
| ColorDepth | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The color depth of the output. | String | GET | N/A |
| ColorSpaceMode | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | Sets the color space for the output. | String | GET, POST | N/A |
| HorizontalBezelCompensation | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | Sets or gets the horizontal bezel compensation for the scaler. | Numeric (Unsigned 16-bit) | GET, POST | N/A |
| VerticalBezelCompensation | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | Sets or gets the vertical bezel compensation for the scaler. | Numeric (Unsigned 16-bit) | GET, POST | N/A |
| VideoTimeout | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.2.0 | Sets or gets the video timeout. | Numeric (Unsigned 16-bit) | GET, POST | N/A |
| IsVideoTimeoutEnabled | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.2.0 | Indicates whether video timeout is enabled (true) or not (false). | Boolean | GET | N/A |
| Underscan | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | Sets the underscan of the device (used to fit to a display.) DM NVX: Range is 0–10%, in 0.1% steps, | Floating Point Number | GET, POST | "0 to 15" |
| MaxColorDepth | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | Sets or gets the maximum color depth. | String | GET, POST | N/A |
| AspectRatioMode | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | Sets or gets the aspect ratio mode. | String | GET, POST | N/A |
| HorizontalResolution | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The horizontal resolution detected on the output. | Numeric (Unsigned 16-bit) | GET | N/A |
| VerticalResolution | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The vertical resolution detected on the output. | Numeric (Unsigned 16-bit) | GET | N/A |
| FramesPerSecond | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The FPS (frames per second) detected on the output. | Numeric (Unsigned 16-bit) | GET | N/A |
| AspectRatio | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The aspect ratio detected on the output. | String | GET | N/A |
| Resolution | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.4.10 | The resolution of the output. | String | GET, POST | Available resolutions vary by device. |
| ResolutionOptionsName | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.3 | Contains the name of the options group supported on this device. | String | GET | N/A |
| ResolutionOptionsVersion | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.3 | Contains the version of the options group supported on this device. | String | GET | N/A |
| Orientation | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.1.0 | Specifies the layout orientation. | String | GET, POST | "Landscape", "Portrait" |
| DownstreamEdid | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | Contains the supported EDIDs for a particular input. | Object Collection | GET, POST | N/A |
| ManufacturerString | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/DownstreamEdid/ | 2.0.0 | The EDID name. | String | GET | N/A |
| SerialNumberString | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/DownstreamEdid/ | 2.0.0 | The EDID type ("System") | String | GET | N/A |
| NameString | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/DownstreamEdid/ | 2.0.0 | The EDID author. | String | GET | N/A |
| PrefTimingString | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/DownstreamEdid/ | 2.0.0 | The EDID unique ID. | String | GET | N/A |
| Hdmi | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The HDMI video object. | Object | GET, POST | N/A |
| IsBlankingDisabled | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Hdmi/ | 2.0.0 | Indicates whether blanking is enabled or disabled. | Boolean | GET, POST | N/A |
| DisabledByHdcp | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Hdmi/ | 2.0.0 | Indicates whether the output is disabled by HDPC. | Boolean | GET | N/A |
| IsOutputDisabled | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Hdmi/ | 2.0.0 | Indicates whether the output is enabled or disabled. | Boolean | GET, POST | N/A |
| HdcpState | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Hdmi/ | 2.0.0 | The current HDCP state. | String | GET | N/A |
| IsCecInError | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Hdmi/ | 2.0.0 | Indicates whether there is a CEC error. true = A CEC error is present, false = No error is present | Boolean | GET | N/A |
| ReceiveCecMessage | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Hdmi/ | 2.0.0 | The serial data received via CEC. | String | GET | N/A |
| HdcpTransmitterMode | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Hdmi/ | 2.0.1 | Sets or gets the HDCP mode for the output. | String | GET, POST | "Auto", "FollowInput", "Always", "Never" |
| UnsupportedVideoDetected | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Hdmi/ | 2.4.0 | Indicates whether an unsupported video type is detected (true) or not (false). | Boolean | GET | N/A |
| CecControl | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.2.0 | The CEC control subobject. | Object | GET, POST | N/A |
| TransmitCecMessage | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/CecControl/ | 2.2.0 | Transmits serial data via CEC. | String | POST | Base64 Encoded Data |
| Format | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/CecControl/ | 2.2.0 | Indicates the CEC message format. | String | GET, POST | "Hex", "Ascii" |
| Delimiter | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/CecControl/ | 2.2.0 | Indicates the CEC message delimiter. | String | GET, POST | "NONE", "CR", "LF", "CR_LF" |
| Type | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/CecControl/ | 2.2.0 | Indicates the CEC message type. | String | GET, POST | "Custom", "Power off: RCP and SS", "Power off: RCP Only", "Power off: SS Only", "Power On: RCP and IVO", "Power On: RCP", "Power On: Image View on" |
| InputControl | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/CecControl/ | 2.2.0 | The input control subobject for standard commands. | Object | GET, POST | N/A |
| TransmitCecMessage | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/CecControl/InputControl/ | 2.2.0 | Transmits serial data via CEC (for input control). | String | POST | Base64 Encoded Data |
| Format | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/CecControl/InputControl/ | 2.2.0 | Indicates the CEC message format (for input control. | String | GET, POST | "Hex", "Ascii" |
| Delimiter | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/CecControl/InputControl/ | 2.2.0 | Indicates the CEC message delimiter (for input control. | String | GET, POST | "NONE", "CR", "LF", "CR_LF" |
| Audio | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/ | 2.0.0 | The audio object. | Object | GET, POST | N/A |
| Volume | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Audio/ | 2.0.0 | Sets the gain of the audio output. | Numeric (Unsigned 16-bit) | GET, POST | "-100 to 100" |
| Digital | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Audio/ | 2.0.0 | The Digital Audio object. | Object | GET | N/A |
| Channels | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Audio/Digital/ | 2.0.0 | The number of audio channels. | Numeric (Unsigned 16-bit) | GET | N/A |
| Format | /Device/AudioVideoInputOutput/Outputs/x/Ports/x/Audio/Digital/ | 2.0.0 | The audio format. | String | GET | N/A |
| Version | /Device/AudioVideoInputOutput/ | 2.4.10 | The object version. "2.0.0": Initial public release, "2.0.1": Added HdcpTransmitterMode in Output Section, "2.0.2": Added EndpointExists for Inputs and Outputs, "2.0.3": Added ResolutionOptionsName, ResolutionOptionsVersion for Outputs, "2.1.0": Added Orientation for Outputs, "2.2.0": Added VideoTimeout properties and CecControl object, InputControl subobject, "2.3.0": Added EndpointId, IsEndPointOnline, and DM endpoint power control object support, "2.4.0": Added UnsupportedVideoDetected property in both HDMI input and output, "2.4.10": Added Resolution refresh rates beyond 60Hz: 1920x1080@120 & 1920x1080@240 | String | GET | N/A |
