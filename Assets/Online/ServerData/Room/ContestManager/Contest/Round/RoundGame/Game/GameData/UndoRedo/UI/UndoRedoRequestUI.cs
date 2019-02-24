﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UndoRedo;

public class UndoRedoRequestUI : UIBehavior<UndoRedoRequestUI.UIData>
{

    #region UIData

    public class UIData : Data
    {

        public VP<ReferenceData<UndoRedoRequest>> undoRedoRequest;

        #region Sub

        public abstract class Sub : Data
        {

            public abstract UndoRedoRequest.State.Type getType();

            public abstract bool processEvent(Event e);

        }

        public VP<Sub> sub;

        #endregion

        #region showAnimation

        public VP<ShowAnimationUI.UIData> showAnimation;

        public void OnHide()
        {
            UndoRedoRequestUI undoRedoRequestUI = this.findCallBack<UndoRedoRequestUI>();
            if (undoRedoRequestUI != null)
            {
                undoRedoRequestUI.back();
            }
            else
            {
                Debug.LogError("undoRedoRequestUI null");
            }
        }

        #endregion

        #region Constructor

        public enum Property
        {
            undoRedoRequest,
            sub,
            showAnimation
        }

        public UIData() : base()
        {
            this.undoRedoRequest = new VP<ReferenceData<UndoRedoRequest>>(this, (byte)Property.undoRedoRequest, new ReferenceData<UndoRedoRequest>(null));
            this.sub = new VP<Sub>(this, (byte)Property.sub, null);
            // showAnimation
            {
                this.showAnimation = new VP<ShowAnimationUI.UIData>(this, (byte)Property.showAnimation, new ShowAnimationUI.UIData());
                this.showAnimation.v.onHide.v = OnHide;
            }
        }

        #endregion

        public bool processEvent(Event e)
        {
            bool isProcess = false;
            {
                // sub
                if (!isProcess)
                {
                    Sub sub = this.sub.v;
                    if (sub != null)
                    {
                        isProcess = sub.processEvent(e);
                    }
                    else
                    {
                        Debug.LogError("sub null");
                    }
                }
                // back
                if (!isProcess)
                {
                    if (InputEvent.isBackEvent(e))
                    {
                        UndoRedoRequestUI undoRedoRequestUI = this.findCallBack<UndoRedoRequestUI>();
                        if (undoRedoRequestUI != null)
                        {
                            undoRedoRequestUI.onClickBtnBack();
                            isProcess = true;
                        }
                        else
                        {
                            Debug.LogError("undoRedoRequestUI null");
                        }
                    }
                }
            }
            return isProcess;
        }

    }

    #endregion

    #region Refresh

    private bool needShowAnimation = false;
    public Button btnBack;

    public override void refresh()
    {
        if (dirty)
        {
            dirty = false;
            if (this.data != null)
            {
                // needShowAnimation
                {
                    if (needShowAnimation)
                    {
                        needShowAnimation = false;
                        ShowAnimationUI.UIData showAnimationUIData = this.data.showAnimation.v;
                        if (showAnimationUIData != null)
                        {
                            ShowAnimationUI.Show show = new ShowAnimationUI.Show();
                            {
                                show.uid = showAnimationUIData.state.makeId();
                            }
                            showAnimationUIData.state.v = show;
                        }
                        else
                        {
                            Debug.LogError("showAnimationUIData null");
                        }
                    }
                }
                UndoRedoRequest undoRedoRequest = this.data.undoRedoRequest.v.data;
                if (undoRedoRequest != null)
                {
                    UndoRedoRequest.State state = undoRedoRequest.state.v;
                    if (state != null)
                    {
                        switch (undoRedoRequest.state.v.getType())
                        {
                            case UndoRedoRequest.State.Type.None:
                                {
                                    None none = state as None;
                                    // UIData
                                    NoneUI.UIData noneUIData = this.data.sub.newOrOld<NoneUI.UIData>();
                                    {
                                        noneUIData.none.v = new ReferenceData<None>(none);
                                    }
                                    this.data.sub.v = noneUIData;
                                }
                                break;
                            case UndoRedoRequest.State.Type.Ask:
                                {
                                    Ask ask = state as Ask;
                                    // UIData
                                    AskUI.UIData askUIData = this.data.sub.newOrOld<AskUI.UIData>();
                                    {
                                        askUIData.ask.v = new ReferenceData<Ask>(ask);
                                    }
                                    this.data.sub.v = askUIData;
                                }
                                break;
                            default:
                                Debug.LogError("unknown state: " + undoRedoRequest.state.v + "; " + this);
                                break;
                        }
                    }
                    else
                    {
                        Debug.LogError("state null: " + this);
                    }
                }
                else
                {
                    // Debug.LogError ("undoRedoRequest null: " + this);
                }
                // SiblingIndex
                {
                    UIRectTransform.SetSiblingIndex(this.data.sub.v, 0);
                    if (btnBack != null)
                    {
                        btnBack.transform.SetSiblingIndex(1);
                    }
                    else
                    {
                        Debug.LogError("btnBack null");
                    }
                }
                // UI
                {
                    float deltaY = 0;
                    // sub
                    deltaY += UIRectTransform.SetPosY(this.data.sub.v, deltaY);
                    // set
                    UIRectTransform.SetHeight((RectTransform)this.transform, deltaY);
                }
            }
            else
            {
                // Debug.LogError ("data null: " + this);
            }
        }
    }

    public override bool isShouldDisableUpdate()
    {
        return true;
    }

    #endregion

    #region implement callBacks

    public NoneUI nonePrefab;
    public AskUI askPrefab;

    public ShowAnimationUI showAnimationUI;

    public override void onAddCallBack<T>(T data)
    {
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Child
            {
                uiData.undoRedoRequest.allAddCallBack(this);
                uiData.sub.allAddCallBack(this);
                uiData.showAnimation.allAddCallBack(this);
            }
            dirty = true;
            return;
        }
        // Child
        {
            if (data is UndoRedoRequest)
            {
                needShowAnimation = true;
                dirty = true;
                return;
            }
            // sub
            {
                if (data is UIData.Sub)
                {
                    UIData.Sub sub = data as UIData.Sub;
                    // UI
                    {
                        switch (sub.getType())
                        {
                            case UndoRedoRequest.State.Type.None:
                                {
                                    NoneUI.UIData none = sub as NoneUI.UIData;
                                    UIUtils.Instantiate(none, nonePrefab, this.transform);
                                }
                                break;
                            case UndoRedoRequest.State.Type.Ask:
                                {
                                    AskUI.UIData ask = sub as AskUI.UIData;
                                    UIUtils.Instantiate(ask, askPrefab, this.transform);
                                }
                                break;
                            default:
                                Debug.LogError("unknown type: " + sub.getType() + "; " + this);
                                break;
                        }
                    }
                    // Child
                    {
                        TransformData.AddCallBack(sub, this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if (data is TransformData)
                {
                    dirty = true;
                    return;
                }
            }
            if (data is ShowAnimationUI.UIData)
            {
                ShowAnimationUI.UIData showAnimationUIData = data as ShowAnimationUI.UIData;
                // UI
                {
                    if (showAnimationUI != null)
                    {
                        showAnimationUI.setData(showAnimationUIData);
                    }
                    else
                    {
                        Debug.LogError("showAnimationUI null");
                    }
                }
                dirty = true;
                return;
            }
        }
        Debug.LogError("Don't process: " + data + "; " + this);
    }

    public override void onRemoveCallBack<T>(T data, bool isHide)
    {
        if (data is UIData)
        {
            UIData uiData = data as UIData;
            // Child
            {
                uiData.undoRedoRequest.allRemoveCallBack(this);
                uiData.sub.allRemoveCallBack(this);
                uiData.showAnimation.allRemoveCallBack(this);
            }
            this.setDataNull(uiData);
            return;
        }
        // Child
        {
            if (data is UndoRedoRequest)
            {
                return;
            }
            // sub
            {
                if (data is UIData.Sub)
                {
                    UIData.Sub sub = data as UIData.Sub;
                    // Child
                    {
                        TransformData.RemoveCallBack(sub, this);
                    }
                    // UI
                    {
                        switch (sub.getType())
                        {
                            case UndoRedoRequest.State.Type.None:
                                {
                                    NoneUI.UIData none = sub as NoneUI.UIData;
                                    none.removeCallBackAndDestroy(typeof(NoneUI));
                                }
                                break;
                            case UndoRedoRequest.State.Type.Ask:
                                {
                                    AskUI.UIData ask = sub as AskUI.UIData;
                                    ask.removeCallBackAndDestroy(typeof(AskUI));
                                }
                                break;
                            default:
                                Debug.LogError("unknown type: " + sub.getType() + "; " + this);
                                break;
                        }
                    }
                    return;
                }
                // Child
                if (data is TransformData)
                {
                    return;
                }
            }
            if (data is ShowAnimationUI.UIData)
            {
                ShowAnimationUI.UIData showAnimationUIData = data as ShowAnimationUI.UIData;
                // UI
                {
                    if (showAnimationUI != null)
                    {
                        showAnimationUI.setDataNull(showAnimationUIData);
                    }
                    else
                    {
                        Debug.LogError("showAnimationUI null");
                    }
                }
                return;
            }
        }
        Debug.LogError("Don't process: " + data + "; " + this);
    }

    public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
    {
        if (WrapProperty.checkError(wrapProperty))
        {
            return;
        }
        if (wrapProperty.p is UIData)
        {
            switch ((UIData.Property)wrapProperty.n)
            {
                case UIData.Property.undoRedoRequest:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                        dirty = true;
                    }
                    break;
                case UIData.Property.sub:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                        dirty = true;
                    }
                    break;
                case UIData.Property.showAnimation:
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                        dirty = true;
                    }
                    break;
                default:
                    Debug.LogError("unknown wrapProperty: " + wrapProperty + "; " + this);
                    break;
            }
            return;
        }
        // Child
        {
            if (wrapProperty.p is UndoRedoRequest)
            {
                switch ((UndoRedoRequest.Property)wrapProperty.n)
                {
                    case UndoRedoRequest.Property.state:
                        dirty = true;
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // sub
            {
                if (wrapProperty.p is UIData.Sub)
                {
                    return;
                }
                // Child
                if (wrapProperty.p is TransformData)
                {
                    switch ((TransformData.Property)wrapProperty.n)
                    {
                        case TransformData.Property.position:
                            break;
                        case TransformData.Property.rotation:
                            break;
                        case TransformData.Property.scale:
                            break;
                        case TransformData.Property.size:
                            dirty = true;
                            break;
                        default:
                            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
            }
            if (wrapProperty.p is ShowAnimationUI.UIData)
            {
                return;
            }
        }
        Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
    }

    #endregion

    public void onClickBtnBack()
    {
        if (this.data != null)
        {
            if (showAnimationUI != null)
            {
                ShowAnimationUI.UIData showAnimationUIData = this.data.showAnimation.v;
                if (showAnimationUIData != null)
                {
                    if ((showAnimationUIData.state.v is ShowAnimationUI.Normal))
                    {
                        ShowAnimationUI.Hide hide = new ShowAnimationUI.Hide();
                        {
                            hide.uid = showAnimationUIData.state.makeId();
                        }
                        showAnimationUIData.state.v = hide;
                    }
                    else
                    {
                        Debug.LogError("state error: " + showAnimationUIData.state.v);
                    }
                }
                else
                {
                    Debug.LogError("showAnimationUIData null");
                }
            }
            else
            {
                Debug.LogError("showAnimationUI null");
                back();
            }
        }
        else
        {
            Debug.LogError("data null");
        }
    }

    public void back()
    {
        if (this.data != null)
        {
            GameUI.UIData gameUIData = this.data.findDataInParent<GameUI.UIData>();
            if (gameUIData != null)
            {
                gameUIData.undoRedoRequestUIData.v = null;
            }
            else
            {
                Debug.LogError("gameUIData null");
            }
        }
        else
        {
            Debug.LogError("data null");
        }
    }

}