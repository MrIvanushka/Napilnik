﻿public void Enable()
{
    _enable = true;
    _effects.StartEnableAnimation();
}

public void Disable()
{
    _enable = false;
    _pool.Free(this);
}