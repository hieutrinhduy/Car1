using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>{
    static T instance;
    public static T Ins { get => instance; }
    [SerializeField] 
    bool needDontDestroy = false;
    public virtual void Awake(){
        if (instance == null){
            instance = (T)this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
        if (needDontDestroy) DontDestroyOnLoad(gameObject);
        SetUp();
    }
    void OnDestroy(){
        if (instance == this){
            instance = null;
        }
    }
    protected virtual void SetUp(){

    }
}