import React from 'react';

interface LevelProps {
  level: number;
  setLevel: (level: number) => void;
  setGridSize: (rows: number, columns: number) => void;
  setMines: (mines: number) => void;
}

const levels = [
  { name: 'Beginner', rows: 9, columns: 9, mines: 10 },
  { name: 'Intermediate', rows: 18, columns: 18, mines: 40 },
  { name: 'Expert', rows: 60, columns: 60, mines: 90 },
];

const Level: React.FC<LevelProps> = ({ level, setLevel, setGridSize, setMines }) => {
  const handleLevelChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const newLevel = Number(event.target.value);
    setLevel(newLevel);

    const levelConfig = levels[newLevel];
    if (levelConfig) {
      setGridSize(levelConfig.rows, levelConfig.columns);
      setMines(levelConfig.mines);
    }
  };

  return (
    <div>
      <label htmlFor="level">Level: </label>
      <select id="level" value={level} onChange={handleLevelChange}>
        {levels.map((level, index) => (
          <option key={index} value={index}>
            {level.name}
          </option>
        ))}
      </select>
    </div>
  );
};

export default Level;