import './App.css';
import Table from './components/table/table';
import { Routes, Route, Link } from 'react-router-dom';

function App() {
  return (
    <div className="App">   
      <nav>
        <Link to="/">Home</Link>
      </nav> 
      <Routes>
        <Route path="" element={<Table/>} />
      </Routes>
    </div>
  );
}

export default App;
