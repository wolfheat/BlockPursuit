using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectExtensions
{
    // Enables keyboard to be used in scrollrect - centering the selected item correctly
    public static Vector2 GetSnapToVerticalPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
    {
        // exit if not valid
        if (instance == null || child == null) return new Vector2();

        Canvas.ForceUpdateCanvases();
        
        // Initial position of the parent (the childs parents position is set in this extension method)
        // This start position is set to the middle of the viewport
        Vector2 startPosition = child.parent.localPosition;
        Vector2 childLocalPosition = child.localPosition;

        // Have a padding on bottom and top when moving a child into view
        float padding = 10f;

        float childHeight = child.rect.height;
        float viewportHeight = instance.viewport.rect.height;
        
        float parentPositionFromTopY = startPosition.y - viewportHeight/2;
        float childsUpperPartPositionY = childLocalPosition.y + parentPositionFromTopY;
        float childsLowerPartPositionY = childLocalPosition.y + parentPositionFromTopY - childHeight;

        bool isBelowViewPort = (-childsLowerPartPositionY) > viewportHeight;
        bool isAboveViewPort = (-childsUpperPartPositionY) < 0;
        
        if (isBelowViewPort)
        {
            float amountBelow = (-childsLowerPartPositionY) - viewportHeight; 
            return startPosition + new Vector2(0,amountBelow+padding);
        }
        else if (isAboveViewPort)
        {
            float amountAbove = childsUpperPartPositionY;
            return startPosition - new Vector2(0,amountAbove+padding);
        }
        return startPosition;
    }
    
    public static Vector2 GetSnapToVerticalPositionToBringChildIntoViewOLD(this ScrollRect instance, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();
        
        Debug.Log("---------------------------: ");
        // Force to top row
        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Debug.Log("Viewports position: "+ viewportLocalPosition);
        Vector2 childLocalPosition = child.localPosition;
        Debug.Log("Childs position: " + childLocalPosition);
        Vector2 thisRectPosition = instance.GetComponent<RectTransform>().localPosition;
        Debug.Log("Scrollrects position: " + thisRectPosition);

        float padding = instance.content.gameObject.GetComponent<GridLayoutGroup>().padding.top;
        float correction = instance.viewport.rect.height / 2; //107.15f;
        float contentPosition = instance.content.localPosition.y-correction;
        Debug.Log("padding: " + padding+ " correction (half height): "+ correction+ " Contents position:: "+ contentPosition);

        float parentTopPos = 0;
        float parentBottomPos = instance.viewport.rect.height;
        float childBottomPos = -child.localPosition.y + child.rect.height;
        float childTopPos = -child.localPosition.y;

       // Childs position when taking contents position into account
        float childBottomPosAdjusted = childBottomPos - contentPosition;
        float childTopPosAdjusted = childTopPos - contentPosition;
        Debug.Log("Childs bottom after adjustion: " + childBottomPosAdjusted + " Childs top after adjustment: " + childTopPosAdjusted);
        
        // If child is outside of viewport scroll it inside
        bool isBelow = childBottomPosAdjusted > instance.viewport.rect.height;
        if (isBelow) Debug.Log("Childs is below viewport!");
        bool isAbove = childTopPosAdjusted < parentTopPos;
        if (isAbove) Debug.Log("Childs is above viewport!");
        
        // If below check how much below and move up that amount


        //Debug.Log("Below "+isBelow +" childPos: "+childLocalPosition.y+" ScrollHeight: "+instance.viewport.rect.height);

        Vector2 newPos = new Vector2();

        if (isBelow)
        {
            float amountBelow = childBottomPosAdjusted - parentBottomPos + padding;
            newPos = new Vector2(viewportLocalPosition.x, correction + contentPosition + amountBelow);
            return newPos;
        }
        if (isAbove)
        {
            float amountBelow = childTopPosAdjusted - parentTopPos - padding;
            newPos = new Vector2(viewportLocalPosition.x, correction + contentPosition + amountBelow);
            return newPos;
        }

        // THis is Good
        return new Vector2(0, contentPosition + correction);
        //return new Vector2(viewportLocalPosition.x, viewportLocalPosition.y+0);
    }
}