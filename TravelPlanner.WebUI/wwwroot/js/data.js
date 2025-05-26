
  const travelTypes = [
    "Chill Seeker",
    "Nature Enthusiast",
    "Culture Explorer",
    "Urban Explorer",
    "Foodie Adventurer",
    "Night Owl",
    "The All-Around Explorer"
  ];
  
  const personalityMap = {
    quiz_1: {
      "relax": ["Chill Seeker", "Nature Enthusiast"],
      "culture": ["Culture Explorer", "Urban Explorer"],
      "vibes": ["Urban Explorer", "Night Owl"],
      "nature": ["Nature Enthusiast", "Chill Seeker"],
      "food": ["Foodie Adventurer", "Culture Explorer"],
      "nightlife": ["Night Owl", "Foodie Adventurer"]
    },
    quiz_2: {
      "landmarks": ["Culture Explorer", "Nature Enthusiast"],
      "parks": ["Nature Enthusiast", "Chill Seeker"],
      "cafes": ["Foodie Adventurer", "Chill Seeker"],
      "museums": ["Culture Explorer", "Foodie Adventurer"],
      "markets": ["Urban Explorer", "Night Owl"],
      "bars": ["Night Owl", "Urban Explorer"]
    },
    quiz_3: {
      "0-1": ["Chill Seeker", "Night Owl"],
      "1-2": ["Chill Seeker", "Foodie Adventurer"],
      "3-5": ["Foodie Adventurer", "Urban Explorer"],
      "6-8": ["Culture Explorer", "Urban Explorer"],
      "9+": ["Culture Explorer", "Nature Enthusiast"],
      "depends": ["Nature Enthusiast", "Night Owl"]
    },
    quiz_4: {
      "indoor": ["Chill Seeker", "Culture Explorer"],
      "outdoor": ["Nature Enthusiast", "Urban Explorer"],
      "mixed": ["Foodie Adventurer", "Culture Explorer"],
      "weather": ["Chill Seeker", "Night Owl"],
      "social": ["Night Owl", "Urban Explorer"],
      "solo": ["Foodie Adventurer", "Nature Enthusiast"]
    },
    quiz_5: {
      "under50": ["Chill Seeker", "Foodie Adventurer"],
      "50-100": ["Culture Explorer", "Nature Enthusiast"],
      "100-200": ["Foodie Adventurer", "Urban Explorer"],
      "over200": ["Night Owl", "Culture Explorer"],
      "nobudget": ["Urban Explorer", "Night Owl"],
      "planner": ["Chill Seeker", "Nature Enthusiast"]
    },
    quiz_6: {
      "cafe": ["Chill Seeker", "Urban Explorer"],
      "museums": ["Culture Explorer", "Nature Enthusiast"],
      "dish": ["Foodie Adventurer", "Culture Explorer"],
      "wander": ["Urban Explorer", "Night Owl"],
      "trail": ["Nature Enthusiast", "Chill Seeker"],
      "party": ["Night Owl", "Foodie Adventurer"]
    },
    quiz_7: {
      "relaxing": ["Chill Seeker", "Nature Enthusiast"],
      "local": ["Foodie Adventurer", "Culture Explorer"],
      "trendy": ["Urban Explorer", "Night Owl"],
      "historic": ["Culture Explorer", "Chill Seeker"],
      "trail": ["Nature Enthusiast", "Foodie Adventurer"],
      "vibes": ["Night Owl", "Urban Explorer"]
    },
    quiz_8: {
      "relax": ["Chill Seeker", "Nature Enthusiast"],
      "culture": ["Culture Explorer", "Chill Seeker"],
      "cuisine": ["Foodie Adventurer", "Night Owl"],
      "shopping": ["Urban Explorer", "Foodie Adventurer"],
      "nature": ["Nature Enthusiast", "Culture Explorer"],
      "nightlife": ["Night Owl", "Urban Explorer"]
    },
    quiz_9: {
      "wellness": ["Chill Seeker", "Nature Enthusiast"],
      "culture": ["Culture Explorer", "Urban Explorer"],
      "cooking": ["Foodie Adventurer", "Culture Explorer"],
      "street": ["Urban Explorer", "Foodie Adventurer"],
      "hike": ["Nature Enthusiast", "Chill Seeker"],
      "party": ["Night Owl", "Night Owl"]
    },
    quiz_10: {
      "peaceful": ["Chill Seeker", "Nature Enthusiast"],
      "smarter": ["Culture Explorer", "Foodie Adventurer"],
      "stuffed": ["Foodie Adventurer", "Chill Seeker"],
      "finds": ["Urban Explorer", "Night Owl"],
      "sunburn": ["Nature Enthusiast", "Urban Explorer"],
      "hungover": ["Night Owl", "Culture Explorer"]
    }
  };

  const typeInfo = {
    "Chill Seeker": {
      quote: "Slow travel, slow vibes, soul restored.",
      traits: [
        "Travels to relax and recharge",
        "Seeks peaceful, cozy, or spiritual environments"
      ],
      description: "You're a laid-back traveler who enjoys slow-paced days and serene spots."
    },
    "Culture Explorer": {
      quote: "Feed me museums, give me goosebumps at the opera.",
      traits: [
        "Intellectual curiosity",
        "Wants to learn and experience history, art, and local culture"
      ],
      description: "You're all about immersing yourself in culture, history, and the arts."
    },
    "Foodie Adventurer": {
      quote: "Taste first, ask questions later.",
      traits: [
        "Culinary-focused travel",
        "Loves variety, spice, and local authenticity"
      ],
      description: "You love exploring new destinations one bite at a time."
    },
    "Urban Explorer": {
      quote: "I wanna get lost in the city… and find a Zara.",
      traits: [
        "Loves big city energy",
        "Interested in iconic spots, people-watching, shopping"
      ],
      description: "You're energized by city streets, shops, and spontaneous discoveries."
    },
    "Nature Enthusiast": {
      quote: "The mountain is calling and I must answer.",
      traits: [
        "Values green spaces and open air",
        "Prioritizes physical wellness in nature"
      ],
      description: "You're happiest surrounded by nature, fresh air, and scenic views."
    },
    "Night Owl": {
      quote: "Sunlight? I don’t know her.",
      traits: [
        "Thrives after sunset",
        "Prefers nightlife, crowds, and good vibes"
      ],
      description: "You're most alive when the city lights turn on and the party begins."
    },
    "All-Around Explorer": {
      quote: "A little bit of this, a little bit of that.",
      traits: [
        "Hard to pin down",
        "Likes variety and trying new things"
      ],
      description: "You're a flexible adventurer who enjoys balance and variety."
    }
  };

  const userData = {
    firstName: localStorage.getItem("user_firstName"),
    lastName: localStorage.getItem("user_lastName"),
    age: localStorage.getItem("user_age"),
    gender: localStorage.getItem("user_gender"),
    email: localStorage.getItem("user_email"),
    password: localStorage.getItem("user_password"),
    quiz: {
      q1: localStorage.getItem("quiz_1"),            
      q2: localStorage.getItem("quiz_2"),        
      q3: localStorage.getItem("quiz_3"),          
      q4: localStorage.getItem("quiz_4"),        
      q5: localStorage.getItem("quiz_5"),    
      q6: localStorage.getItem("quiz_6"),                
      q7: localStorage.getItem("quiz_7"),                
      q8: localStorage.getItem("quiz_8"),                
      q9: localStorage.getItem("quiz_9"),                
      q10: localStorage.getItem("quiz_10")               
    }
  };

 