# Standard Assets for Unity

## Introduction

These are standard assets that I include in most of my projects. Though this package was put together with a third person RPG in mind, it could be helpful for other genres as well.

This package is under the MIT license (with some content originally from the public domain). This license was chosen so that you can grab what you need with the least amount of hassle possible.


### Required Dependency

#### Unity Tweens

I do not include Unity Tweens since it's still actively maintained by the original creator. Please get the latest version of Unity Tweens here: [link](https://github.com/jeffreylanters/unity-tweens)

Since Unity Tweens is MIT licensed, popular, and actively maintained, I do not see a reason to let this be an optional dependency.

### Optional Dependencies

These are proprietary assets that I currently use. Many users do not like proprietary assets, so I have included instruction here on how to remove these dependencies. However, by default, this StandardAssets package assumes these dependencies have already been included.

#### Dialogue System for Unity

Direcory StandardAssets/Script/Savers includes saver components. In order to remove dependency on the Dialogue System for Unity, remove this directory.

- Dialogue System for Unity [link](https://assetstore.unity.com/packages/tools/behavior-ai/dialogue-system-for-unity-11672)

#### MicroSplat

Direcory StandardAssets/Art/MicroSplat includes a Texture Array Configuration and material for MicroSplat, though you will need to acquire the MicroSplat shader file from the Unity Asset Store.

If you do not want to use this dependency, simply delete the MicroSplat folder, then update the demo terrain's material with your favorite terrain shader.

- MicroSplat [link](https://assetstore.unity.com/packages/tools/terrain/microsplat-96478)
- MicroSplat URP 2022 [link](https://assetstore.unity.com/packages/tools/terrain/microsplat-urp-2022-support-244845)

## Scenes

### Demo

There's not much to the demo scene, yet. This may be removed.

![Demo](./Documentation/Image/Demo.png)

### Layout

An overview of included art.

![Layout](./Documentation/Image/Layout.png)