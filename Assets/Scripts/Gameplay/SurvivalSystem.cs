using UnityEngine;

/// <summary>
/// Manages survival mechanics: resource tracking, weather, challenge progression.
/// </summary>
public class SurvivalSystem : MonoBehaviour
{
    [System.Serializable]
    public class Resource
    {
        public string name;
        public int current;
        public int maxCapacity;
        public int dailyLoss;
    }

    [System.Serializable]
    public class WeatherState
    {
        public string weatherType; // sunny, cloudy, rainy, stormy
        public float duration;
        public float elapsedTime;
    }

    private Resource water = new() { name = "Water", current = 5, maxCapacity = 5, dailyLoss = 1 };
    private Resource food = new() { name = "Food", current = 10, maxCapacity = 10, dailyLoss = 2 };
    private Resource shelter = new() { name = "Shelter", current = 100, maxCapacity = 100, dailyLoss = 2 };

    private WeatherState currentWeather;
    private int dayCounter = 1;
    private int challengeDayCounter = 1;
    private bool isChallengeDue = false;

    public void StartDaySimulation()
    {
        Debug.Log("[SurvivalSystem] Day simulation started");
        ConsumeResources();
        RandomizeWeather();
        CheckChallengeTrigger();
    }

    private void ConsumeResources()
    {
        water.current -= water.dailyLoss;
        food.current -= food.dailyLoss;
        shelter.current -= shelter.dailyLoss;

        if (water.current < 0) water.current = 0;
        if (food.current < 0) food.current = 0;
        if (shelter.current < 0) shelter.current = 0;

        Debug.Log($"[SurvivalSystem] Resources consumed - Water: {water.current}, Food: {food.current}, Shelter: {shelter.current}");
    }

    private void RandomizeWeather()
    {
        string[] weatherTypes = { "sunny", "cloudy", "rainy", "stormy" };
        currentWeather = new()
        {
            weatherType = weatherTypes[Random.Range(0, weatherTypes.Length)],
            duration = Random.Range(30f, 120f),
            elapsedTime = 0
        };

        // Weather effects
        if (currentWeather.weatherType == "rainy")
            water.current += 2; // Rain refills water
        else if (currentWeather.weatherType == "stormy")
            shelter.current -= 10; // Storm damages shelter

        Debug.Log($"[SurvivalSystem] Weather: {currentWeather.weatherType}");
    }

    public void GatherResource(string resourceType, int amount)
    {
        switch (resourceType)
        {
            case "water":
                water.current = Mathf.Min(water.current + amount, water.maxCapacity);
                break;
            case "food":
                food.current = Mathf.Min(food.current + amount, food.maxCapacity);
                break;
        }
        Debug.Log($"[SurvivalSystem] Gathered {amount} {resourceType}");
    }

    public void ConsumeResource(string resourceType, int amount)
    {
        switch (resourceType)
        {
            case "water":
                water.current -= amount;
                break;
            case "food":
                food.current -= amount;
                break;
        }
    }

    public void BuildShelter(int durabilityBoost)
    {
        shelter.current = Mathf.Min(shelter.current + durabilityBoost, shelter.maxCapacity);
        Debug.Log($"[SurvivalSystem] Shelter upgraded to {shelter.current}");
    }

    public bool CheckChallengeTrigger()
    {
        // Challenge every 3 days or when resources critical
        if (dayCounter % 3 == 0 || water.current < 2 || food.current < 3)
        {
            isChallengeDue = true;
            Debug.Log("[SurvivalSystem] Challenge triggered!");
            return true;
        }
        return false;
    }

    public void StartChallenge(int challengeDay)
    {
        challengeDayCounter = challengeDay;
        IncreaseDifficulty();
        Debug.Log($"[SurvivalSystem] Challenge started - Day {challengeDay}/7");
    }

    public void IncreaseDifficulty()
    {
        // Scale up resource loss by challenge day
        float difficultyMultiplier = 1f + (challengeDayCounter * 0.2f);
        water.dailyLoss = (int)(1 * difficultyMultiplier);
        food.dailyLoss = (int)(2 * difficultyMultiplier);

        Debug.Log($"[SurvivalSystem] Difficulty multiplier: {difficultyMultiplier}x");
    }

    public Resource GetResource(string resourceType)
    {
        return resourceType switch
        {
            "water" => water,
            "food" => food,
            "shelter" => shelter,
            _ => null
        };
    }

    public void AdvanceDay()
    {
        dayCounter++;
        StartDaySimulation();
    }

    public int GetCurrentDay() => dayCounter;

    /// <summary>Current weather type, or empty until the first day simulation runs.</summary>
    public string GetWeatherName() => currentWeather?.weatherType ?? "";
}
