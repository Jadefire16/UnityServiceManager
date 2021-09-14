# The Unity Service Manager
### _So Easy To Use You'll Forget It's There_

The Unity Service Manager is a simple tool built for Unity. It creates a robust and easy to use Service Manager that allows quick and easy creation of singleton instances within Unitys' environment. Relavtively useful for things such as Game, Audio or Scene managers which require an instance to exist in the scene however need to act static.

### Getting Started

- Download the package or install core files from github
  https://github.com/Jadefire16/UnityServiceManager/tree/main/Assets/Scripts/Core
- Make a script you want to turn into a service
- Instead of inheriting from Monobehaviour, inherit from ServiceBase<T> where T is the current class

A quick example would be something like this
```cs
public class GameManager : ServiceBase<GameManager>
{
    //Really thats it!
}
```

##### What happens next?
#
Well through a litte bit of reflection magic the SceneManager class packed into the Core files will hunt down all of the scripts in your project that inherit from that ServiceBase class, from there it works a little bit more magic and creates an object for your services to live on, stores them on that object and marks the objct to not be destroyed when scene loading.

##### What Do I Do After That?
#
Next you have to actually get references to those services in your code, say you had a script which needed to access your scene manager to let it know something happened, here's how you could go about doing it.
```cs
public class MyTriggerClass : MonoBehaviour
{
    public void OnTriggerEnter(Collider other){
        ServiceManager.GetService<SceneManager>().TriggerCollided(other);
    }
}
```

Pretty neat huh? We've now called the TriggerCollided function in our SceneManager and our SceneManager is well on it's way to doing what it needs to do next, but what if we needed to call a function lots? Like on a score manager? Getting that component over and over would be pretty silly so let's try something else...
```cs
public class ScoreCounter : ServiceBase<ScoreCounter>
{
    private int currentScore;
    private UIManager uiManager;
    
    private void Start(){
        uiManager = ServiceManager.GetService<UIManager>();
    }
    
    public void ScoreIncreased(int amount){
        currentScore += amount;
        uiManager.UpdateScoreUI(currentScore);
    }
}
```
Here we actually have two services running together at the same time, and our ScoreCounter manager is referencing our UIManager to let it know when our score updates! But how? Well we store a reference to the UIManager in our ScoreCounter class and assign it to the value we return when we call GetService()!

Last we have the...
```cs
[DoNotInitializeOnLoad]
```
... Attribute which when placed above any class, that inherits ServiceBase, will let the ServiceManager know not to initialize this service! this can be useful if you want to do the setup yourself or need to have a different way of managing things but still want to use the ServiceManager!

There's a few other features you can play around with, feel free to take a look at whats there to offer!


