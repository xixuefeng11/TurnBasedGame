﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using Seirawan.UseRule;

namespace Seirawan
{
    public class UseRuleInputUI : UIBehavior<UseRuleInputUI.UIData>
    {

        #region UIData

        public class UIData : InputUI.UIData.Sub
        {

            public VP<ReferenceData<Seirawan>> seirawan;

            #region State

            public abstract class State : Data
            {

                public enum Type
                {
                    /** Get legal moves*/
                    Get,
                    /** Getting legal moves*/
                    Getting,
                    /** Have got legal moves, now show*/
                    Show
                }

                public abstract Type getType();

                public abstract bool processEvent(Event e);

            }

            public VP<State> state;

            #endregion

            #region Constructor

            public enum Property
            {
                seirawan,
                state
            }

            public UIData() : base()
            {
                this.seirawan = new VP<ReferenceData<Seirawan>>(this, (byte)Property.seirawan, new ReferenceData<Seirawan>(null));
                this.state = new VP<State>(this, (byte)Property.state, new GetUI.UIData());
            }

            #endregion

            public override Type getType()
            {
                return Type.UseRule;
            }

            public override bool processEvent(Event e)
            {
                bool isProcess = false;
                {
                    // state
                    if (!isProcess)
                    {
                        State state = this.state.v;
                        if (state != null)
                        {
                            isProcess = state.processEvent(e);
                        }
                        else
                        {
                            Debug.LogError("state null: " + this);
                        }
                    }
                }
                return isProcess;
            }

        }

        #endregion

        public override int getStartAllocate()
        {
            return Setting.get().defaultChosenGame.v.getGame() == GameType.Type.Seirawan ? 1 : 0;
        }

        #region Refresh

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Seirawan seirawan = this.data.seirawan.v.data;
                    if (seirawan != null)
                    {
                        // if seirawan have change, return to get
                        if (haveChange)
                        {
                            haveChange = false;
                            // getUIData
                            {
                                GetUI.UIData getUIData = this.data.state.newOrOld<GetUI.UIData>();
                                {

                                }
                                this.data.state.v = getUIData;
                            }
                        }
                        // Task get ai move
                        switch (this.data.state.v.getType())
                        {
                            case UIData.State.Type.Get:
                                {
                                    destroyRoutine(getLegalMoves);
                                    // Chuyen sang getting
                                    {
                                        GettingUI.UIData newGetting = new GettingUI.UIData();
                                        {
                                            newGetting.uid = this.data.state.makeId();
                                        }
                                        this.data.state.v = newGetting;
                                    }
                                }
                                break;
                            case UIData.State.Type.Getting:
                                {
                                    if (Routine.IsNull(getLegalMoves))
                                    {
                                        getLegalMoves = CoroutineManager.StartCoroutine(TaskGetLegalMoves(), this.gameObject);
                                    }
                                    else
                                    {
                                        Debug.LogError("Why getLegalMoves != null");
                                    }
                                }
                                break;
                            case UIData.State.Type.Show:
                                {
                                    destroyRoutine(getLegalMoves);
                                }
                                break;
                            default:
                                Debug.LogError("unknown type: " + this.data.state.v.getType() + "; " + this);
                                break;
                        }
                    }
                    else
                    {
                        Debug.LogError("seirawan null: " + this);
                        GetUI.UIData getUIData = this.data.state.newOrOld<GetUI.UIData>();
                        {

                        }
                        this.data.state.v = getUIData;
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
            return false;
        }

        #endregion

        #region Task get legal moves

        private Routine getLegalMoves;

        public IEnumerator TaskGetLegalMoves()
        {
            // Find seirawan
            Seirawan seirawan = null;
            {
                if (this.data != null)
                {
                    if (this.data.seirawan.v.data != null)
                    {
                        seirawan = this.data.seirawan.v.data;
                    }
                    else
                    {
                        Debug.LogError("seirawan null: " + this);
                    }
                }
                else
                {
                    Debug.LogError("data null: " + this);
                }
            }
            if (seirawan != null)
            {
                List<SeirawanMove> legalMoves = Core.unityGetLegalMoves(seirawan, Core.CanCorrect);
                // Wait
                yield return new Wait();
                // Change state
                {
                    // Find show
                    ShowUI.UIData show = this.data.state.newOrOld<ShowUI.UIData>();
                    {
                        // legalMoves
                        {
                            // Debug.LogError ("show legalMoves: " + GameUtils.Utils.getListString (legalMoves));
                            show.legalMoves.clear();
                            for (int i = 0; i < legalMoves.Count; i++)
                            {
                                SeirawanMove legalMove = legalMoves[i];
                                {
                                    legalMove.uid = show.legalMoves.makeId();
                                }
                                show.legalMoves.add(legalMove);
                            }
                        }
                    }
                    this.data.state.v = show;
                }
            }
            else
            {
                Debug.LogError("seirawan null: " + this);
            }
        }

        public override List<Routine> getRoutineList()
        {
            List<Routine> ret = new List<Routine>();
            {
                ret.Add(getLegalMoves);
            }
            return ret;
        }

        #endregion

        #region implement callBacks

        public GetUI getPrefab;
        public GettingUI gettingPrefab;
        public ShowUI showPrefab;

        /** seirawan have change?*/
        private bool haveChange = true;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.seirawan.allAddCallBack(this);
                    uiData.state.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // State
            {
                if (data is UIData.State)
                {
                    UIData.State state = data as UIData.State;
                    {
                        switch (state.getType())
                        {
                            case UIData.State.Type.Get:
                                {
                                    GetUI.UIData myGet = state as GetUI.UIData;
                                    UIUtils.Instantiate(myGet, getPrefab, this.transform);
                                }
                                break;
                            case UIData.State.Type.Getting:
                                {
                                    GettingUI.UIData getting = state as GettingUI.UIData;
                                    UIUtils.Instantiate(getting, gettingPrefab, this.transform);
                                }
                                break;
                            case UIData.State.Type.Show:
                                {
                                    ShowUI.UIData show = state as ShowUI.UIData;
                                    UIUtils.Instantiate(show, showPrefab, this.transform);
                                }
                                break;
                            default:
                                Debug.LogError("unknown type: " + state.getType());
                                break;
                        }
                    }
                    dirty = true;
                    return;
                }
            }
            // Seirawan
            {
                if (data is Seirawan)
                {
                    Seirawan seirawan = data as Seirawan;
                    // Child
                    {
                        seirawan.addCallBackAllChildren(this);
                    }
                    dirty = true;
                    haveChange = true;
                    return;
                }
                // Child
                {
                    // if (data.findDataInParent<Seirawan> () != null) 
                    {
                        data.addCallBackAllChildren(this);
                        dirty = true;
                        haveChange = true;
                        return;
                    }
                }
            }
            // Debug.LogError ("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.seirawan.allRemoveCallBack(this);
                    uiData.state.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // State
            {
                if (data is UIData.State)
                {
                    UIData.State state = data as UIData.State;
                    {
                        switch (state.getType())
                        {
                            case UIData.State.Type.Get:
                                {
                                    GetUI.UIData myGet = state as GetUI.UIData;
                                    myGet.removeCallBackAndDestroy(typeof(GetUI));
                                }
                                break;
                            case UIData.State.Type.Getting:
                                {
                                    GettingUI.UIData getting = state as GettingUI.UIData;
                                    getting.removeCallBackAndDestroy(typeof(GettingUI));
                                }
                                break;
                            case UIData.State.Type.Show:
                                {
                                    ShowUI.UIData show = state as ShowUI.UIData;
                                    show.removeCallBackAndDestroy(typeof(ShowUI));
                                }
                                break;
                            default:
                                Debug.LogError("unknown type: " + state.getType());
                                break;
                        }
                    }
                    return;
                }
            }
            // Seirawan
            {
                if (data is Seirawan)
                {
                    Seirawan seirawan = data as Seirawan;
                    // Child
                    {
                        seirawan.removeCallBackAllChildren(this);
                    }
                    return;
                }
                // Child
                {
                    data.removeCallBackAllChildren(this);
                    return;
                }
            }
            // Debug.LogError ("Don't process: " + data + "; " + this);
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
                    case UIData.Property.seirawan:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.state:
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
            // State
            {
                if (wrapProperty.p is UIData.State)
                {
                    return;
                }
            }
            // Seirawan
            {
                if (wrapProperty.p is Seirawan)
                {
                    if (Generic.IsAddCallBackInterface<T>())
                    {
                        ValueChangeUtils.replaceCallBack(this, syncs);
                    }
                    dirty = true;
                    haveChange = true;
                    return;
                }
                // Child
                {
                    {
                        if (Generic.IsAddCallBackInterface<T>())
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                        }
                        dirty = true;
                        haveChange = true;
                        return;
                    }
                }
            }
            // Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}