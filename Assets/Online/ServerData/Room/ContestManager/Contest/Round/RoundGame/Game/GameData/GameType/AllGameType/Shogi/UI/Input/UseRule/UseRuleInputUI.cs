﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedCoroutines;
using Shogi.UseRule;

namespace Shogi
{
    public class UseRuleInputUI : UIBehavior<UseRuleInputUI.UIData>
    {

        #region UIData

        public class UIData : InputUI.UIData.Sub
        {

            public VP<ReferenceData<Shogi>> shogi;

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
                shogi,
                state
            }

            public UIData() : base()
            {
                this.shogi = new VP<ReferenceData<Shogi>>(this, (byte)Property.shogi, new ReferenceData<Shogi>(null));
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
            return Setting.get().defaultChosenGame.v.getGame() == GameType.Type.SHOGI ? 1 : 0;
        }

        #region Refresh

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    Shogi shogi = this.data.shogi.v.data;
                    if (shogi != null)
                    {
                        // if shogi have change, return to get
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
                        Debug.LogError("shogi null: " + this);
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
            // Find shogi
            Shogi shogi = null;
            {
                if (this.data != null)
                {
                    if (this.data.shogi.v.data != null)
                    {
                        shogi = this.data.shogi.v.data;
                    }
                    else
                    {
                        Debug.LogError("shogi null: " + this);
                    }
                }
                else
                {
                    Debug.LogError("shogi null: " + this);
                }
            }
            if (shogi != null)
            {
                List<ShogiMove> legalMoves = Core.unityGetLegalMoves(shogi, Core.CanCorrect);
                // Wait
                yield return new Wait();
                // Change state
                {
                    // Find show
                    ShowUI.UIData showUIData = this.data.state.newOrOld<ShowUI.UIData>();
                    {
                        // legalMoves
                        {
                            // Debug.LogError ("show legalMoves: " + GameUtils.Utils.getListString (legalMoves));
                            showUIData.legalMoves.clear();
                            if (legalMoves.Count > 0)
                            {
                                for (int i = 0; i < legalMoves.Count; i++)
                                {
                                    ShogiMove legalMove = legalMoves[i];
                                    {
                                        legalMove.uid = showUIData.legalMoves.makeId();
                                    }
                                }
                                // Make change
                                {
                                    List<Change<ShogiMove>> changes = new List<Change<ShogiMove>>();
                                    {
                                        ChangeAdd<ShogiMove> changeAdd = new ChangeAdd<ShogiMove>();
                                        {
                                            changeAdd.index = 0;
                                            changeAdd.values.AddRange(legalMoves);
                                        }
                                        changes.Add(changeAdd);
                                    }
                                    showUIData.legalMoves.processChange(changes);
                                }
                            }
                            else
                            {
                                Debug.LogError("Don't have legalMoves: " + this);
                            }
                        }
                    }
                    this.data.state.v = showUIData;
                }
            }
            else
            {
                Debug.LogError("shogi null: " + this);
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

        /** chess have change?*/
        private bool haveChange = true;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {
                    uiData.shogi.allAddCallBack(this);
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
            // Reversi
            {
                if (data is Shogi)
                {
                    Shogi shogi = data as Shogi;
                    // Child
                    {
                        shogi.addCallBackAllChildren(this);
                    }
                    dirty = true;
                    haveChange = true;
                    return;
                }
                // Child
                {
                    // if (data.findDataInParent<Shogi> () != null) 
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
                    uiData.shogi.allRemoveCallBack(this);
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
            // Shogi
            {
                if (data is Shogi)
                {
                    Shogi shogi = data as Shogi;
                    // Child
                    {
                        shogi.removeCallBackAllChildren(this);
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
                    case UIData.Property.shogi:
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
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
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
            // Reversi
            {
                if (wrapProperty.p is Shogi)
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
                    if (wrapProperty.p.findDataInParent<Shogi>() != null)
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
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}