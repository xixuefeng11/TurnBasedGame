﻿using UnityEngine;
using System.Threading;

namespace Reversi
{
    public class TestReversi
    {

        private class Work
        {
            public void DoWork()
            {
                Reversi startReversi = Core.unityMakeDefaultPosition();
                // Make a match
                {
                    int turn = 0;
                    Reversi reversi = startReversi;
                    do
                    {
                        Debug.Log(string.Format("before letComputerThink: {0}", turn));
                        Debug.Log(Common.printPosition(reversi));
                        int gameFinish = Core.unityIsGameFinish(reversi, true);
                        Debug.Log("gameFinish: " + gameFinish);
                        if (gameFinish == 0)
                        {
                            int sort = 4;
                            int min = 4;
                            int max = 8;
                            int end = 4;
                            int msLeft = 24 * 10000;
                            bool useBook = false;
                            int percent = 95;
                            sbyte move = Core.unityLetComputerThink(reversi, true, sort, min, max, end, msLeft, useBook, percent);
                            // print move to string
                            Debug.Log("find ai move: " + Common.printMove(move));
                            {
                                // check legal move
                                if (Core.unityIsLegalMove(reversi, true, move))
                                {
                                    // do move
                                    Reversi newReversi = Core.unityDoMove(reversi, true, move);
                                    // set new position bytes
                                    DataUtils.copyData(reversi, newReversi);
                                }
                                else
                                {
                                    Debug.Log("error: why not legal move: " + move);
                                    break;
                                }
                            }
                            turn++;
                        }
                        else
                        {
                            Debug.LogWarning("game finish in turn: " + turn);
                            Debug.LogWarning(Common.printPosition(reversi));
                            switch (gameFinish)
                            {
                                case 1:
                                    Debug.LogWarning("black win: " + turn);
                                    break;
                                case 2:
                                    Debug.LogWarning("white win: " + turn);
                                    break;
                                case 3:
                                    Debug.LogWarning("the game is draw: " + turn);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                        System.Threading.Thread.Sleep(1000);
                    } while (!Test.stop);
                }
            }
        }

        static void DoWork(object work)
        {
            if (work is Work)
            {
                ((Work)work).DoWork();
            }
            else
            {
                Debug.LogError("why not work: " + work);
            }
        }

        public static void startTestMatch(int matchCount)
        {
            for (int i = 0; i < matchCount; i++)
            {
                Work w = new Work();
                {
                    // startThread
                    /*ThreadStart threadDelegate = new ThreadStart(w.DoWork);
                    Thread newThread = new Thread(threadDelegate, Global.ThreadSize);
                    newThread.Start();*/
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork), w);
                }
            }
        }

    }
}