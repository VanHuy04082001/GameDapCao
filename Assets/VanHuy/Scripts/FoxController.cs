using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
namespace VanHuy
{
    public class FoxController : MonoBehaviour
    {
        public enum TypeFox {FoxNormal,FoxGold,FoxFreeze,FoxMuscle,FoxHat,FoxX2Score,FoxThor,FoxPoison,FoxBoom}
        public TypeFox typeFox;
        public int indexFox;
        [SpineAnimation]
        public string appear;
        [SpineAnimation]
        public string get_hit1;
        [SpineAnimation]
        public string get_hit2;
        [SpineAnimation]
        public string idle;
        [SpineAnimation]
        public string idle_to_hole;
        [SpineAnimation]
        public string stun_to_hole;
        [SpineAnimation]
        public string fox2_stun;
        [SpineAnimation]
        public string fox2_idle;
        [SpineAnimation]
        public string fox2_idle_to_hole;

        public SkeletonAnimation skeletonAnimation;
        public Spine.AnimationState spineAnimationState;
        public Spine.Skeleton skeleton;
        public int hpFox;
        public Hole hole;
        //public ParticleSystem vfxSmashImpact;
        float timeEndFox = 4;
        float timeFoxHat=1;
        bool isTimeEndFox;
        bool isFoxHat;
        Collider2D coll2D;
        // Start is called before the first frame update
        void Start()
        {
            coll2D = GetComponent<Collider2D>();
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            skeleton = skeletonAnimation.Skeleton;
            ActionAppears();
        }

        // Update is called once per frame
        void Update()
        {
            if (isTimeEndFox)
            {
                return;
            }    
            CheckTimeEndFox();
            CheckTimeFoxHat();
        }
        public void ActionAppears() //animation cao xuat hien
        {
            spineAnimationState.SetAnimation(0, appear, false); 
            spineAnimationState.AddAnimation(0, idle, true, 0);
        }
        public void CheckTimeEndFox()
        {
            if(isTimeEndFox == false)
            {
                timeEndFox -= Time.deltaTime;
                if(timeEndFox<=0)
                {
                    if(hpFox>0)
                    {
                        Spine.TrackEntry trackEntry  = spineAnimationState.AddAnimation(0, idle_to_hole, false,0);
                        OnAnimationComplete(trackEntry);
                    }
                    else
                    {
                        spineAnimationState.AddAnimation(0, stun_to_hole, false, 0);
                    }
                    timeEndFox = 4;
                    isTimeEndFox = true;
                }    
            }    
        }
        public void CheckTimeFoxHat() // kiem tra thoi gian choang cua cao doi mu
        {
            if (isFoxHat)
            {
                timeFoxHat -= Time.deltaTime;
                if (timeFoxHat <= 0)
                {
                    spineAnimationState.AddAnimation(0, idle, true, 0);
                    SetCollider(true);
                    timeFoxHat = 0;
                    isFoxHat = false;
                }
            }
        }
        public void ActionHitFox() // danh trung cao thi xay ra su kien
        {
            hole.vfxSmashImpact.Play();
            SetActionVFX();
            hpFox--; //tru mau
            SetAnimHitFox();
            if (hpFox <= 0) //Hp cua cao nho hon 0 thi cao chui xuong lo ( chet )
            {
                SetCollider(false);
                timeEndFox = 0;
            }
        }
        public void SetAnimHitFox()
        {
            spineAnimationState.SetAnimation(0, get_hit1, false);
            if (hpFox > 0)
            {
                if (typeFox == TypeFox.FoxMuscle)
                {
                    spineAnimationState.AddAnimation(0, idle, true, 0);
                }
                else if (typeFox == TypeFox.FoxHat) // cao doi mu bi danh lan 1 se bi choang va k danh dc nua
                {
                    spineAnimationState.AddAnimation(0, fox2_stun, true, 0);
                    SetCollider(false);
                    isFoxHat = true; //kiem tra xem sau khi bi choang thi bao nhieu s hoi lai  
                }
            }
        }
        void SetActionVFX()
        {
            if (typeFox == TypeFox.FoxGold)
            {
                hole.logicGame.gameController.SpawnVfx(hole.logicGame.gameController.vfxCoin, transform);
            }
            else if (typeFox == TypeFox.FoxFreeze)
            {
                hole.logicGame.gameController.SpawnVfx(hole.logicGame.gameController.vfxFreeze, transform);
                hole.logicGame.gameController.SpawnVfx(hole.logicGame.gameController.vfxSlow, transform);
            }
            else if (typeFox == TypeFox.FoxBoom)
            {
                hole.logicGame.gameController.SpawnVfx(hole.logicGame.gameController.vfxBoom, transform);
                hole.logicGame.gameController.isDie = true;
            }
            else if (typeFox == TypeFox.FoxPoison)
            {
                hole.logicGame.gameController.SpawnVfx(hole.logicGame.gameController.vfxPoison, transform);
            }
            else if (typeFox == TypeFox.FoxX2Score || typeFox == TypeFox.FoxThor)
            {
                hole.logicGame.gameController.SpawnVfx(hole.logicGame.gameController.vfxBox, transform);
            }
        }
        void SetCollider(bool isCheck)
        {
            coll2D.enabled = isCheck;
        }
        private void OnAnimationComplete(Spine.TrackEntry trackEntry)
        {
            // Kiểm tra xem animation đã kết thúc chưa
            if (trackEntry.Animation.Name == "idle_to_hole")
            {
                // Animation đã kết thúc, thực hiện các hành động bạn muốn
                SetCollider(false);
            }
        }
    }
}
