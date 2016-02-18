#Stealth Subsystem
This document is meant to illustrate the ActiveMalware list and how the Stealth subsystem interacts with it

#IMalware Lifecycle
1. `VirusAI.OnAwake` calls `ActiveSubroutines.AddVirus`
  1. The instance is added to `ActiveSubroutines.MalwareList`
  2. If not lurking, the instance is added to the VirusAI's `Machine.ActiveMalware`
  3. Else, the instance is added to `Machine.LurkingMalware`

2. Whenever the `ActiveSubroutines.MalwareList` changes:
  * `InfectionMonitor.OnMalwareListChange` is called
  * If the entire subnet's `MalwareList.Count == 0` then the win condition is fulfilled
  * Else, `VirusIconDisplay.UpdateIcon` is called for all non-lurking `IMalware`
  
3. Whenever a virus is removed from `ActiveSubroutines.MalwareList`
  * If the machine is infected and the machine has 0 `ActiveMalware` then:
    * `IsInfected = false` and Cores are rewarded. 
    * Any `ILurkers` are alerted that their machine is "clean."
    
4. When an `ILurker` is informed that their current machine is "clean"
  * If `WaitUntilWholeSubnetIsClean` then wait until the entire subnet is "clean" before attempting re-infection.
  * Else wait 2-5 minutes before attempting re-infection.
