# HDT Unity Visualizer

This Unity package provides a set of tools and assets for visualizing a SMPL model in a webgl application.

## Requirements

- Unity 6000.0.32f1 or later

## Installation

1. Clone or download this repository to your local disk.
2. Open Unity Hub.
3. Click on the `Add` -> `Add project from disk` button.
4. Select the cloned or downloaded project folder.

## Usage
1. Open the project from unity hub
2. Open the scene 'Scenes/HumanVisualizer'
3. You are ready to modify the project.

## Features

- **Default Avatars**: Pre-configured avatars used in the scene.
- **Materials**: A set of materials to enhance the visual quality of your scenes.
- **Plugins**: Essential plugins.
- **Prefabs**: Ready-to-use prefabs.
- **Samples**: Example scenes from imported packages.
- **Scenes**: Pre-built scenes.
- **Scripts**: Custom scripts.
- **Settings**: Configuration settings to customize the behavior of the package.

## Scripts

The `Scripts` folder contains custom scripts that add functionality to your project. Some of the key scripts include:

- **CharacterSelector**: Manages the selection and loading of character models. It includes functionality to import characters from external sources and activate the selected character in the scene. If the selected model is custom, a glb file will be loaded.
- **GetAvatarBoneTree**: Exports all the bones of the avatar with unique log messages for debugging purposes.
- **JointsStateSubscriber**: Subscribes to joint state messages and updates the character's joints accordingly.
- **JointsStateSubscriber_SMPL**: Similar to `JointsStateSubscriber`, but specifically tailored for SMPL models.
- **PlayerController**: Manages player movement and interaction, including rotation, zoom, and panning.

## Scenes

The `Scenes` folder contains pre-built scenes to help you get started quickly. These scenes are designed to demonstrate the capabilities of the package and provide a foundation for your own projects. Some of the key scenes include:

- **HumanVisualizer**: The main scene that showcases the default avatars and allows for character selection and interaction.

## Plugins

The `Plugins` folder contains essential plugins to extend the functionality of your project. Some of the key plugins include:

- **ConnectToLocalStorage.jslib**: A JavaScript library that provides functions to interact with the browser's local storage, such as retrieving WebSocket URLs and character file paths.

## Dependencies

This package relies on several Unity modules and external packages. Ensure that the following dependencies are included in your project:

- `com.endel.nativewebsocket`
- `com.unity.animation.rigging`
- `com.unity.cloud.gltfast`
- `com.unity.collab-proxy`
- `com.unity.feature.development`
- `com.unity.inputsystem`
- `com.unity.multiplayer.center`
- `com.unity.nuget.newtonsoft-json`
- `com.unity.timeline`
- `com.unity.ugui`
- `com.unity.visualscripting`
- Various Unity built-in modules (e.g., `com.unity.modules.ai`, `com.unity.modules.animation`, etc.)

## Build the project
To build the project in WebGL, there are two profiles:
- **Default WebGL**: Builds the development build.
- **Release**: Builds the release build.

>**Note**: If the project is built for the hdt webapp, build the project in the folder 'hdt_unity_visualizer'.