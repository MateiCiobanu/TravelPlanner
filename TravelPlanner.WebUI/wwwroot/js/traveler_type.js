function determineUserType() {
    const scores = {};
  
    Object.keys(personalityMap).forEach((key) => {
      const answer = localStorage.getItem(key);
      if (answer && personalityMap[key][answer]) {
        personalityMap[key][answer].forEach(type => {
          scores[type] = (scores[type] || 0) + 1;
        });
      }
    });
  
    const sortedTypes = Object.entries(scores).sort((a, b) => b[1] - a[1]);
    const scoreValues = sortedTypes.map(entry => entry[1]);
    const uniqueScores = new Set(scoreValues);
    const isBalanced = uniqueScores.size <= 2 && scoreValues[0] - scoreValues[scoreValues.length - 1] <= 1;
  
    if (isBalanced && sortedTypes.length >= 4) {
      return "All-Around Explorer";
    }
  
    return sortedTypes.length ? sortedTypes[0][0] : null;
  }

  const user_type = determineUserType();

  function calculateTypePercentages() {
    const scores = {};
    let totalMentions = 0;
  
    Object.keys(personalityMap).forEach((key) => {
      const answer = localStorage.getItem(key);
      const types = personalityMap[key][answer];
  
      if (answer && types) {
        types.forEach((type) => {
          scores[type] = (scores[type] || 0) + 1;
          totalMentions++;
        });
      }
    });
  
    const percentages = {};
    Object.entries(scores).forEach(([type, count]) => {
      percentages[type] = ((count / totalMentions) * 100).toFixed(2) + "%";
    });
  
    return percentages;
  }

  const type_percentages = calculateTypePercentages();

