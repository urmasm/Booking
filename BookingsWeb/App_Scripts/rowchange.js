var _oldColor;
function SetNewColor(source)
{
    _oldColor = source.style.backgroundColor;
    source.style.backgroundColor = 'Silver';
    source.style.cursor = 'pointer';
}
function SetOldColor(source)
{
    source.style.backgroundColor = _oldColor;
    source.style.cursor = 'default';
}