﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonSaveObject {

    [HideInInspector]
    public List<Cockpit_Component_Json> components;
    [HideInInspector]
    public List<Container_Box_Json> boxes;

    public JsonSaveObject()
    {
        components = new List<Cockpit_Component_Json>();
        boxes = new List<Container_Box_Json>();
    }
}